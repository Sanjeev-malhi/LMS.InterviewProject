using Azure.Storage.Queues.Models;
using LMS.Application.Events;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace LMS.Functions;

public class UserRegisteredFunction
{
    private readonly ILogger<UserRegisteredFunction> _logger;

    public UserRegisteredFunction(ILogger<UserRegisteredFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(UserRegisteredFunction))]
    public void Run(
    [QueueTrigger("user-registration", Connection = "AzureWebJobsStorage")]
    string message)
    {
        _logger.LogInformation("Message received.");

        _logger.LogInformation(message);

        var user =
            JsonSerializer.Deserialize<UserRegisteredEvent>(message);

        _logger.LogInformation(
            "User : {Email}",
            user?.Email);
    }
}