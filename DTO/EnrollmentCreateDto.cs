using System;

namespace Education.DTO
{
    public class EnrollmentCreateDto
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
    }
}
