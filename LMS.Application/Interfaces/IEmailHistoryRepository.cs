using LMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Interfaces
{
    public interface IEmailHistoryRepository
    {
        Task<bool> ExistsByEventIdAsync(Guid eventId, CancellationToken cancellationToken);

        Task<EmailHistory> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken);

        Task AddAsync(EmailHistory emailHistory, CancellationToken cancellationToken);

        void Update(EmailHistory emailHistory);
    }
}
