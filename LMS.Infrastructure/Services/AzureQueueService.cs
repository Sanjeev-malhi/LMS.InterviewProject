using Azure.Storage.Queues;
using LMS.Application.Configuration;
using LMS.Application.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LMS.Infrastructure.Services
{
    public class AzureQueueService : IAzureQueueService
    {
        private readonly QueueClient _queueClient;
        public AzureQueueService(IOptions<AzureStorageSettings> options)
        {
            var settings = options.Value;
            _queueClient = new QueueClient(settings.ConnectionString, settings.QueueName);
            _queueClient.CreateIfNotExistsAsync();
        }
        public async Task EnqueueAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(message);
            await _queueClient.SendMessageAsync(json, cancellationToken);
        }
    }
}
