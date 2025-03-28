using System;
using System.ComponentModel.DataAnnotations;

namespace Education.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Address { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        // New Role Property
        public string Role { get; set; } = "Student"; // Default role is Student
    }
}
