using LMS.Application.Events;
using LMS.Application.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailHistoryRepository _repository;
        public UserRegistrationProcessor(IEmailService emailService, 
                                         ILogger<UserRegistrationProcessor> logger,
                                         IUnitOfWork unitOfWork,
                                         IEmailHistoryRepository repository)
        {
            _emailService = emailService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        public async Task ProcessAsync(UserRegisteredEvent userEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Precessing Registration For {email}", userEvent.Email);
            await _emailService.SendWelcomeEmailAsync(userEvent.Email, cancellationToken);
            _logger.LogInformation("Registration Processing Completed");
        }
    }
}
