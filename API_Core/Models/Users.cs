namespace API_Core.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PositiveBalance { get; set; }

        public List<Records> Records { get; set; }
    }
}
