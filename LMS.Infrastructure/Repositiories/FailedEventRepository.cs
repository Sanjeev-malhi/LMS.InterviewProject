using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Repositiories
{
    public class FailedEventRepository : IFailedEventRepository
    {
        private readonly ApplicationDbContext _context;
        public FailedEventRepository(ApplicationDbContext context)
        {
            _context = context;    
        }
        public async Task AddAsync(FailedEvents events, CancellationToken cancellationToken)
        {
            await _context.FailedEvents.AddAsync(events, cancellationToken);
        }
    }
}
