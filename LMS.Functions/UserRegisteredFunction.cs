using Azure.Storage.Queues.Models;
using LMS.Application.Events;
using LMS.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace LMS.Functions;

public class UserRegisteredFunction
{
    private readonly ILogger<UserRegisteredFunction> _logger;
    private readonly IUserRegistrationProcessor _processor;

    public UserRegisteredFunction(ILogger<UserRegisteredFunction> logger, IUserRegistrationProcessor processor)
    {
        _logger = logger;
        _processor = processor;
    }

    [Function(nameof(UserRegisteredFunction))]
    public async Task Run(
    [QueueTrigger("user-registration", Connection = "AzureWebJobsStorage")]
    UserRegisteredEvent message,
    FunctionContext context,
    CancellationToken cancellationToken)
    {
        var bindingData = context.BindingContext.BindingData;
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = message.CorrelationId,
            ["EventId"] = message.EventId
        }))
        {
            _logger.LogInformation("Message Id: {MessageId}", bindingData["Id"]);
            _logger.LogInformation("Dequeue Count: {DequeueCount}", bindingData["DequeueCount"]);

            await _processor.ProcessAsync(message, context.CancellationToken);
        }
    }
}