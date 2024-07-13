namespace API_Core.Models.Dtos
{
    public class RecordsDto
    {
        public int UserId { get; set; }
        public string Description { get; set; }
        public decimal UserPaid { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
