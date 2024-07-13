namespace API_Core.Models
{
    public class Records
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal UserPaid { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }

        public Users Users { get; set; }
    }
}
