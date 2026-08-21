using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Repositiories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddEventAsync(Enrollment enrollment, CancellationToken cancellationToken)
        {
            await _context.Enrollments.AddAsync(enrollment);
        }

        public async Task<Enrollment> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return await _context.Enrollments.FirstOrDefaultAsync(x => x.EventId == eventId);
        }

        public void UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken)
        {
            _context.Enrollments.Update(enrollment);
        }
    }
}
