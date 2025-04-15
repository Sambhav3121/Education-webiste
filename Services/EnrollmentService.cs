using Education;
using Education.DTO;
using Education.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Education.Data;

namespace Education.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Enrollment> EnrollUserAsync(EnrollmentCreateDto dto)
        {
            var enrollment = new Enrollment
            {
                EnrollmentId = Guid.NewGuid(),
                UserId = dto.UserId,
                CourseId = dto.CourseId,
                EnrollmentDate = DateTime.UtcNow
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<IEnumerable<Enrollment>> GetUserEnrollmentsAsync(Guid userId)
         {
          return await _context.Enrollments
        .Include(e => e.Course)
        .Where(e => e.UserId == userId && e.UnenrollmentDate == null) 
        .ToListAsync();
        }


        public async Task<bool> UnenrollUserAsync(Guid enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null) return false;

            enrollment.UnenrollmentDate = DateTime.UtcNow;
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
