using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Core.Models;
using API_Core.Models.Dtos;

namespace API_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly AppDB_Context _context;

        public RecordsController(AppDB_Context context)
        {
            _context = context;
        }

        // GET: api/Records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Records>>> GetRecords()
        {
            return await _context.Records.ToListAsync();
        }

        // GET: api/Records/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Records>> GetRecords(int id)
        {
            var records = await _context.Records.FindAsync(id);

            if (records == null)
            {
                return NotFound();
            }

            return records;
        }

        // PUT: api/Records/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecords(int id, Records records)
        {
            if (id != records.Id)
            {
                return BadRequest();
            }

            _context.Entry(records).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordsExists(id))
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

        // POST: api/Records
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostRecords(RecordsDto input)
        {
            Records newRecord = new Records();

            //Mapeo datos a Entidad Record
            newRecord.UserId = input.UserId;
            newRecord.Description = input.Description;
            newRecord.Date = DateTime.Now;
            newRecord.UnitPrice = input.UnitPrice;
            newRecord.UserPaid = input.UserPaid;

            //Saca diferencia del total
            newRecord.Total = input.UnitPrice - input.UserPaid;
            newRecord.Total = -newRecord.Total;

            //Se busca el usuario por medio del Id
            var balanceUser = await _context.Users.FindAsync(input.UserId);

            if (balanceUser == null)
            {
                return NotFound();
            }

            //Se actualiza el balance positivo
            balanceUser.PositiveBalance = newRecord.Total + balanceUser.PositiveBalance;


            if (input.UserId != balanceUser.Id)
            {
                return BadRequest();
            }

            try
            {
                //Se carga en memoria la actualizacion del balance positivo
                _context.Entry(balanceUser).State = EntityState.Modified;
                //Se carga en memoria el nuevo record
                _context.Records.Add(newRecord);

                //Se ejecutan los cambios guardados en memoria y se envia a la BD
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Se agregó el nuevo Registro correctamente" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Algo salió mal", ErrorDetails = e.Message });
            }  
        }

        // DELETE: api/Records/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecords(int id)
        {
            var records = await _context.Records.FindAsync(id);
            if (records == null)
            {
                return NotFound();
            }

            _context.Records.Remove(records);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecordsExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }
    }
}
