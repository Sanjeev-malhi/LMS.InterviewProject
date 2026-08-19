using Azure.Storage.Queues;
using LMS.Application.Configuration;
using LMS.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LMS.Infrastructure.Services
{
    public class AzureQueueService : IAzureQueueService
    {
        private readonly string _connectionString;
        public AzureQueueService(
       IOptions<AzureStorageSettings> options)
        {
            _connectionString = options.Value.ConnectionString;

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException(
                    "Azure Storage connection string is not configured.");
            }
        }
        public async Task EnqueueAsync<T>(string queueName, T message, CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentException("Queue name can not be empty", nameof(queueName));
            }

            var queueClient = new QueueClient(_connectionString, queueName);

            await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);


            var json = JsonSerializer.Serialize(message);
            await queueClient.SendMessageAsync(json, cancellationToken);
        }
    }
}
