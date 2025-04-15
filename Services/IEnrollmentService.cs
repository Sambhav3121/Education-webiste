using Education.DTO;
using Education.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Education.Services
{
    public interface IEnrollmentService
    {
        Task<Enrollment> EnrollUserAsync(EnrollmentCreateDto dto);
        Task<IEnumerable<Enrollment>> GetUserEnrollmentsAsync(Guid userId);
        Task<bool> UnenrollUserAsync(Guid enrollmentId);
    }
}
