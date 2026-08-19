using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Interfaces
{
    /// <summary>
    /// This interface is created as generic, this mean we will use same interface and its message to add Register User Message,
    /// Course Purchase Message, Payment Complete Message without changing the interface (it will stop adding the new message for everyone) 
    /// </summary>
    public interface IAzureQueueService
    {
        Task EnqueueAsync<T>(string queueName, T message, CancellationToken cancellationToken = default);
    }
}
