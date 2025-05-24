using System;
using System.ComponentModel.DataAnnotations;

namespace Education.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public string? Address { get; set; }

        public DateTime BirthDate { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string Role { get; set; } = "Student"; // Default role
    }
}
