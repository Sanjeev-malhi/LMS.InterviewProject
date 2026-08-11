using LMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    }
}
