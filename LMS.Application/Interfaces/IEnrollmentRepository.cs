using LMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken);

        Task AddEventAsync(Enrollment enrollment, CancellationToken cancellationToken);

        void UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken);
    }
}
