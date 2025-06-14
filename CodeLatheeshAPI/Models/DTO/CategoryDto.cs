namespace CodeLatheeshAPI.Models.DTO
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string Date { get; set; }

        public string PaymentMethod { get; set; }

        public string Type { get; set; }
    }
}
