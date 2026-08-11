using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositiories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;    
        }
        public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Courses.AsNoTracking().
            FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
