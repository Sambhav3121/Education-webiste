using System;
using System.ComponentModel.DataAnnotations;

namespace Education.DTO
{
    public class EditUserProfileDto
    {
        [Required]
        public string FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}
