namespace Education.Models
{
    public class Enrollment
    {
        public Guid EnrollmentId { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? UnenrollmentDate { get; set; }

        public User User { get; set; }
        public Course Course { get; set; }
    }
}
