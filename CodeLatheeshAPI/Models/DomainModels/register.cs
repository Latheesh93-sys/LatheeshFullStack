﻿using System.ComponentModel.DataAnnotations;

namespace CodeLatheeshAPI.Models.DomainModels
{
    public class register
    {
        
        public string Username { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
