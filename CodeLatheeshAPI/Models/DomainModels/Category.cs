using System.ComponentModel.DataAnnotations.Schema;

namespace CodeLatheeshAPI.Models.DomainModels
{
    public class Category
    {
        public Guid Id { get; set; }

        // Foreign Key to Users table
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string PaymentMethod { get; set; }

        public string Type { get; set; }

    }
}
