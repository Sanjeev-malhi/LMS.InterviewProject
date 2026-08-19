using Azure;
using LMS.Application.Events;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace LMS.Functions.Functrions;

public class PaymentWebHookFunction
{
    private readonly ILogger<PaymentWebHookFunction> _logger;
    private readonly IAzureQueueService _queueService;

    public PaymentWebHookFunction(ILogger<PaymentWebHookFunction> logger, IAzureQueueService queueService)
    {
        _logger = logger;
        _queueService = queueService;
    }

    [Function("PaymentWebHookFunction")]
    public async Task <HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route ="payment/webhook")] 
                            HttpRequestData req, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Payment WebHook Received.");

        PaymentReceivedEvent paymentEvent;

        try
        {
            paymentEvent = await JsonSerializer.DeserializeAsync<PaymentReceivedEvent>(
                req.Body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                },
                cancellationToken);

        }
        catch (JsonException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid payment webhook JSON.");

            var badRequest = req.CreateResponse(
                HttpStatusCode.BadRequest);

            await badRequest.WriteStringAsync(
                "Invalid request body.",
                cancellationToken);

            return badRequest;
        }

        if(paymentEvent == null)
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Request Body is required", cancellationToken);
            return badRequest;
        }

        if(paymentEvent.EventId == Guid.Empty ||
            paymentEvent.CourseId == Guid.Empty ||
            paymentEvent.UserId == Guid.Empty ||
            string.IsNullOrWhiteSpace(paymentEvent.TransactionId) ||
            paymentEvent.Amount <= 0)
        {
            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Invalid Payment Event", cancellationToken);
            return badRequest;
        }

        await _queueService.EnqueueAsync("payment-events", paymentEvent, cancellationToken);
        _logger.LogInformation(
            "Payment event queued. EventId: {EventId}, TransactionId: {TransactionId}",
            paymentEvent.EventId,
            paymentEvent.TransactionId);

        var response = req.CreateResponse(HttpStatusCode.Accepted);
        await response.WriteAsJsonAsync(
            new
            {
                Message = "Payment received and queued.",
                EventId = paymentEvent.EventId
            },
            cancellationToken);

        return response;
    }
}