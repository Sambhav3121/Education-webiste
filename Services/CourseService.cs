using Education.DTO;
using Education.Models;
using Education.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Education.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Course> CreateCourseAsync(CourseCreateDto dto, string teacherId)
        {
            // Parse teacherId string to Guid
            if (!Guid.TryParse(teacherId, out Guid teacherGuid))
                throw new ArgumentException("Invalid teacher ID");

            // Find category by name (case-insensitive)
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.CategoryName.ToLower());

            if (category == null)
                throw new Exception($"Category '{dto.CategoryName}' not found.");

            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = category.Id,
                TeacherId = teacherGuid
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }
    }
}
