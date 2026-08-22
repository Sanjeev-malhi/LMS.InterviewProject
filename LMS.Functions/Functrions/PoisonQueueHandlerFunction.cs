using Azure.Messaging;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Functions.Functrions
{
    public class PoisonQueueHandlerFunction
    {
        private readonly ILogger<PoisonQueueHandlerFunction> _logger;
        private readonly IFailedEventRepository _repository;
        private readonly IUnitOfWork _unitOfWoork;

        public PoisonQueueHandlerFunction(ILogger<PoisonQueueHandlerFunction> logger,
                                          IFailedEventRepository repository,
                                          IUnitOfWork unitOfWoork)
        {
            _unitOfWoork = unitOfWoork;
            _repository = repository;
            _logger = logger;
        }

        // Watches the payment-events poison queue
        [Function("PaymentEventsPoisonHandler")]
        public async Task RunPaymentPoison(
            [QueueTrigger("payment-events-poison", Connection = "AzureWebJobsStorage")]
        string poisonMessage,
            CancellationToken cancellationToken)
        {
            await SaveFailedEventAsync("payment-events", poisonMessage, cancellationToken);
        }

        // Watches the user-registration poison queue
        [Function("UserRegistrationPoisonHandler")]
        public async Task RunUserRegistrationPoison(
            [QueueTrigger("user-registration-poison", Connection = "AzureWebJobsStorage")]
        string poisonMessage,
            CancellationToken cancellationToken)
        {
            await SaveFailedEventAsync("user-registration", poisonMessage, cancellationToken);
        }

        private async Task SaveFailedEventAsync(string queueName, string messageContent, CancellationToken cancellationToken)
        {
            _logger.LogError(
            "Message permanently failed after max retries. Queue: {QueueName}, Content: {Content}",
            queueName, messageContent);

            var failedEvent = new FailedEvents
            {
                Id = new Guid(),
                QueueName = queueName,
                MessageContent = messageContent,
                FailedOn = DateTime.UtcNow
            };

            await _repository.AddAsync(failedEvent, cancellationToken);
            await _unitOfWoork.SaveChangesAsync(cancellationToken);
        }
    }
}
