using Education.DTO;
using Education.Models;
using Education.Data;
using System.Threading.Tasks;

namespace Education.Services
{
    public class LessonService : ILessonService
    {
        private readonly ApplicationDbContext _context;

        public LessonService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Updated method signature to include teacherId
        public async Task<Lesson> CreateLessonAsync(LessonCreateDto dto, string teacherId)
        {
            var lesson = new Lesson
            {
                Title = dto.Title,
                Content = dto.Content,         // make sure Lesson model has Content property
                VideoUrl = dto.VideoUrl,
                CourseId = dto.CourseId
                // You can add TeacherId here if Lesson model supports it, e.g. TeacherId = teacherId
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }
    }
}
