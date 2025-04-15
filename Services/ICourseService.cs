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
        Task<PaginatedResultDto<CourseDto>> GetAllCoursesAsync(int page = 1, int pageSize = 10, string category = null, string search = null);
        Task<CourseDto> GetCourseByIdAsync(Guid courseId);
        Task DeleteCourseAsync(Guid courseId);
    }
}
