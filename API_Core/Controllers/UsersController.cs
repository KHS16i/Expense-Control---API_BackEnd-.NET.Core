using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Core.Models;
using Microsoft.AspNetCore.Cors;

namespace API_Core.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDB_Context _context;

        public UsersController(AppDB_Context context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/UserBalance
        [HttpGet("GetUsersBalance")]
        public async Task<ActionResult<Users>> GetUsersBalance()
        {
            decimal balanceUser1, balanceUser2, total;

            var usersBalance1 = await _context.Users.FindAsync(1);
            var usersBalance2 = await _context.Users.FindAsync(2);

            if (!(usersBalance1 == null || usersBalance2 == null))
            {
                balanceUser1 = usersBalance1.PositiveBalance;
                balanceUser2 = usersBalance2.PositiveBalance;

                //Realiza la resta de los balances positivos los dos usuarios
                total = balanceUser1 - balanceUser2;

                //Valida que si la variable total da 0 entonces el balance positivo para los dos usuarios es 0
                if (total == 0)
                {
                    usersBalance1.PositiveBalance = 0;
                    usersBalance2.PositiveBalance = 0;

                    usersBalance1.Name = string.Empty;

                    return usersBalance1;
                }
                //Valida que si la variable total es un numero negativo esto quiere decir que el balance positivo debe asignarse al usuario 2
                else if (total < 0)
                {
                    //El metodo Math.Abs devuelve solo un numero absoluto, por que convierte el valor de la variable total a positivo
                    usersBalance1.PositiveBalance = 0;
                    usersBalance2.PositiveBalance = Math.Abs(total);

                    return usersBalance2;
                }
                //Si no se cumplen ninguna de las dos condiciones entonces nos indica que el balance positivo debe asignarse al usuario 1
                else
                {
                    usersBalance1.PositiveBalance = total;
                    usersBalance2.PositiveBalance = 0;

                    return usersBalance1;
                }
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
