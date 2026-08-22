using LMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Interfaces
{
    public interface IFailedEventRepository
    {
        Task AddAsync(FailedEvents events, CancellationToken cancellationToken);
    }
}
