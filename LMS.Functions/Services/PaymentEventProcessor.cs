using LMS.Application.Events;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using LMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
namespace LMS.Functions.Services
{
    public class PaymentEventProcessor : IPaymentEventProcessor
    {
        private readonly IEnrollmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentEventProcessor> _logger;
        public PaymentEventProcessor(IEnrollmentRepository enrollmentRepository, IUnitOfWork unitOfWork, ILogger<PaymentEventProcessor> logger)
        {
            _repository = enrollmentRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ProcessAsync(PaymentReceivedEvent paymentEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Processing Payment Received Event. EventId: {EventId}, UserId: {UserId}, CorrelationId: {CorrelationId}",
                 paymentEvent.EventId, paymentEvent.UserId, paymentEvent.CorrelationId);

            var existingPaymentEvent = await _repository.GetByEventIdAsync(paymentEvent.EventId, cancellationToken);

            if(existingPaymentEvent is not null)
            {
                if(existingPaymentEvent.Status == EnrollmentStatus.Active)
                {
                    _logger.LogInformation("Enrollment already active. Skipping duplicate. EventId: {EventId}", paymentEvent.EventId);
                    return;
                }
                await ActiveAsync(existingPaymentEvent, cancellationToken);
                return;
            }
            var enrollment = new Enrollment
            {
                Id = Guid.NewGuid(),
                EventId = paymentEvent.EventId,
                CorrelationId = paymentEvent.CorrelationId,
                UserId = paymentEvent.UserId,
                CourseId = paymentEvent.CourseId,
                TransactionId = paymentEvent.TransactionId,
                AmountPaid = paymentEvent.Amount,
                Status = EnrollmentStatus.Pending,
                CreatedOn = DateTime.UtcNow
            };

            try
            {
                await _repository.AddEventAsync(enrollment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when(IsUniqueConstraintViolation(ex))
            {
                _logger.LogInformation("Concurrent duplicate payment event detected. EventId: {EventId}", paymentEvent.EventId);
                return;
            }
            await ActiveAsync(enrollment, cancellationToken);
        }

        private async Task ActiveAsync(Enrollment existingEvent, CancellationToken cancellationToken)
        {
            try
            {
                existingEvent.Status = EnrollmentStatus.Active;
                existingEvent.CreatedOn = DateTime.UtcNow;

                _repository.UpdateAsync(existingEvent, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Enrollment activated. EventId: {EventId}, UserId: {UserId}, CourseId: {CourseId}",
                        existingEvent.EventId, existingEvent.UserId, existingEvent.CourseId);
            }
            catch (Exception ex)
            {
                existingEvent.ErrorMessage = ex.Message;
                existingEvent.RetryCount++;
                existingEvent.Status = EnrollmentStatus.Failed;
                existingEvent.LastModifiedOn = DateTime.UtcNow;

                _repository.UpdateAsync(existingEvent, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogError(ex, "Enrollment activation failed. EventId: {EventId}", existingEvent.EventId);
                throw;
            }
            
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex) =>
            ex.InnerException?.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) == true ||
            ex.InnerException?.Message.Contains("UNIQUE constraint", StringComparison.OrdinalIgnoreCase) == true;
    }
}
