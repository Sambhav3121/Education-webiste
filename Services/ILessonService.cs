using Education.DTO;
using Education.Models;
using System.Threading.Tasks;

namespace Education.Services
{
    public interface ILessonService
    {
        Task<Lesson> CreateLessonAsync(LessonCreateDto dto, string teacherId);
    }
}
