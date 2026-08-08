using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Repositiories
{
    public class EmailHistoryRepository : IEmailHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        public EmailHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(EmailHistory emailHistory, CancellationToken cancellationToken)
        {
            await _context.EmailHistories.AddAsync(emailHistory, cancellationToken);
        }

        public async Task<bool> ExistsByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return await _context.EmailHistories.AnyAsync(x => x.EventId == eventId, cancellationToken);
        }

        public async Task<EmailHistory> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return await _context.EmailHistories.FirstOrDefaultAsync(x => x.EventId == eventId, cancellationToken);
        }

        public void Update(EmailHistory emailHistory)
        {
            _context.EmailHistories.Update(emailHistory);
        }
    }
}
