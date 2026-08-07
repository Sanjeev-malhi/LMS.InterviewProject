using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Functions.Services
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(
            string email,
            CancellationToken cancellationToken = default);
    }
}
