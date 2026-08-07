using LMS.Application.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Functions.Services
{
    public class UserRegistrationProcessor : IUserRegistrationProcessor
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<UserRegistrationProcessor> _logger;
        public UserRegistrationProcessor(IEmailService emailService, ILogger<UserRegistrationProcessor> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }
        public async Task ProcessAsync(UserRegisteredEvent userEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Precessing Registration For {email}", userEvent.Email);
            await _emailService.SendWelcomeEmailAsync(userEvent.Email, cancellationToken);
            _logger.LogInformation("Registration Processing Completed");
        }
    }
}
