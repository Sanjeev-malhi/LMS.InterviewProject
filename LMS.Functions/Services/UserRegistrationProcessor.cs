using LMS.Application.Events;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
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
            _logger.LogInformation("Precessing User Registration. EventId: {EventId}, UserId: {UserId}", 
                                   userEvent.EventId, userEvent.UserId);

            var existingEmailHistory =
                await _repository.GetByEventIdAsync(
                userEvent.EventId,
                cancellationToken);

            EmailHistory emailHistory;

            if (existingEmailHistory != null)
            {
                if (existingEmailHistory.Status == EmailStatus.Send)
                {
                    _logger.LogInformation(
                        "Email already successfully processed. " +
                        "Skipping duplicate event. EventId: {EventId}",
                        userEvent.EventId);

                    return;
                }

                _logger.LogInformation(
                    "Existing email record found with status {Status}. " +
                    "Retrying email processing. EventId: {EventId}",
                    existingEmailHistory.Status,
                    userEvent.EventId);

                emailHistory = existingEmailHistory;
            }

            else
            {
                emailHistory = new EmailHistory
                {
                    Id = Guid.NewGuid(),
                    EventId = userEvent.EventId,
                    UserId = userEvent.UserId,
                    Email = userEvent.Email,
                    EmailType = EmailType.Welcome,
                    Subject = "Welcome to LMS",
                    Body = $"Welcome to LMS, {userEvent.Email}",
                    Status = EmailStatus.Pending,
                    RetryCount = 0,
                    CreatedOn = DateTime.UtcNow
                };

                await _repository.AddAsync(
                    emailHistory,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(
                    cancellationToken);
            }

            try
            {
                await _emailService.SendWelcomeEmailAsync(userEvent.Email, cancellationToken);

                emailHistory.Status = EmailStatus.Send;
                emailHistory.SentOn = DateTime.UtcNow;
                emailHistory.LastModifiedOn = DateTime.UtcNow;

                _repository.Update(emailHistory);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                "Email successfully processed. EventId: {EventId}",
                userEvent.EventId);

            }
            catch (Exception ex)
            {
                emailHistory.RetryCount++;
                emailHistory.ErrorMessage = ex.Message;
                emailHistory.Status = EmailStatus.Failed;
                emailHistory.LastModifiedOn = DateTime.UtcNow;

                _repository.Update(emailHistory);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogError(ex,
                "Email processing failed. EventId: {EventId}",
                userEvent.EventId);

                throw;
            }
        }
    }
}
