using System.ComponentModel.DataAnnotations;

namespace CodeLatheeshAPI.Models.DomainModels
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<Category> Categories { get; set; } // Navigation property
    }
}
