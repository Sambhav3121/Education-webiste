using Education.Data;
using Education.DTO;
using Education.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Course> CreateCourseAsync(CreateCourseDto createCourseDto)
        {
            var course = new Course
            {
                Title = createCourseDto.Title,
                Description = createCourseDto.Description,
                InstructorId = createCourseDto.InstructorId,
                Category = createCourseDto.Category,
                Price = createCourseDto.Price
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<List<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            var instructorIds = courses.Select(c => c.InstructorId).Distinct().ToList();
            var instructors = await _context.Users
                .Where(u => instructorIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.FullName);

            return courses.Select(course => new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                InstructorName = instructors.ContainsKey(course.InstructorId) ? instructors[course.InstructorId] : "Unknown",
                Category = course.Category,
                Price = course.Price,
                CreatedAt = course.CreatedAt
            }).ToList();
        }

        public async Task<CourseDto> GetCourseByIdAsync(Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null) throw new KeyNotFoundException("Course not found.");

            var instructor = await _context.Users.FirstOrDefaultAsync(u => u.Id == course.InstructorId);

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                InstructorName = instructor?.FullName ?? "Unknown",
                Category = course.Category,
                Price = course.Price,
                CreatedAt = course.CreatedAt
            };
        }

        public async Task DeleteCourseAsync(Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null) throw new KeyNotFoundException("Course not found.");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
