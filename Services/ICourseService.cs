using Education.DTO;
using Education.Models;
using System.Threading.Tasks;

namespace Education.Services
{
    public interface ICourseService
    {
        Task<Course> CreateCourseAsync(CourseCreateDto dto, string teacherId);
    }
}
