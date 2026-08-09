using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Functions.Services
{
    public class MockEmailService : IEmailService
    {
        private readonly ILogger<MockEmailService> _logger;
        public MockEmailService(ILogger<MockEmailService> logger)
        {
            _logger = logger;
        }
        public async Task SendWelcomeEmailAsync(string email, CancellationToken cancellationToken = default)
        {
                _logger.LogInformation("Welcome email has been sent carefully");
            await Task.CompletedTask;
        }
    }
}
