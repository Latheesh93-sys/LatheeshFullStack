namespace CodeLatheeshAPI.Models.DTO
{
    public class UpdateCategoryRequestDto
    {
        public string Name { get; set; }
        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string PaymentMethod { get; set; }

        public string Type { get; set; }
    }
}
