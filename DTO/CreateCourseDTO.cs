using System;
using System.ComponentModel.DataAnnotations;

namespace Education.DTO
{
    public class CreateCourseDto
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid InstructorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
        public decimal Price { get; set; }
    }
}
