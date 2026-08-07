using LMS.Application.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Functions.Services
{
    public interface IUserRegistrationProcessor
    {
        Task ProcessAsync(UserRegisteredEvent userEvent, CancellationToken cancellationToken);
    }
}
