using Education.DTO;
using Education.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Education.Services
{
    public interface ICourseService
    {
        Task<Course> CreateCourseAsync(CreateCourseDto createCourseDto);
        Task<List<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto> GetCourseByIdAsync(Guid courseId);
        Task DeleteCourseAsync(Guid courseId);
    }
}
