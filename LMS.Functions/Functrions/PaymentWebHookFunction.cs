using Azure;
using LMS.Application.Events;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace LMS.Functions.Functrions;

public class PaymentWebHookFunction
{
    private readonly ILogger<PaymentWebHookFunction> _logger;
    private readonly IAzureQueueService _queueService;
    private readonly IWebhookSignatureValidator _signatureValidator;
    private readonly IConfiguration _configuration;

    public PaymentWebHookFunction(ILogger<PaymentWebHookFunction> logger, IAzureQueueService queueService,
                                 IWebhookSignatureValidator signatureValidator, IConfiguration configuration)
    {
        _logger = logger;
        _queueService = queueService;
        _signatureValidator = signatureValidator;
        _configuration = configuration;        
    }

    [Function("PaymentWebHookFunction")]
    public async Task <HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", 
                            Route ="payment/webhook")] HttpRequestData req, FunctionContext context,
                            CancellationToken cancellationToken)
    {
        _logger.LogInformation("Payment WebHook Received.");
        string rawBody = string.Empty;
        using( var reader = new StreamReader(req.Body))
        {
            rawBody = await reader.ReadToEndAsync(cancellationToken);
        }

        if(!req.Headers.TryGetValues("X-Razorpay-Signature", out var signatureValue))
        {
            _logger.LogWarning("Payment Webhook Rejected, missing signature header");
            var missingSigResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
            await missingSigResponse.WriteStringAsync("Missing signature", cancellationToken);
            return missingSigResponse;
        }

        //var receivedSignature = signatureValue.FirstOrDefault();
        //var secret = _configuration["PaymentWebhook:Secret"];
        //if (!_signatureValidator.IsValid(rawBody, receivedSignature, secret))
        //{
        //    _logger.LogWarning("Payment webhook rejected: signature verification failed.");
        //    var invalidSigResponse = req.CreateResponse(HttpStatusCode.BadRequest);
        //    await invalidSigResponse.WriteStringAsync("Invalid Signature", cancellationToken);
        //    return invalidSigResponse;
        //}

        PaymentReceivedEvent paymentEvent;
        try
        {
            paymentEvent = JsonSerializer.Deserialize<PaymentReceivedEvent>(
                rawBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Invalid payment webhook JSON.");

            var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequest.WriteStringAsync("Invalid request body.", cancellationToken);
            return badRequest;
        }

        if (paymentEvent == null)
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

        paymentEvent.CorrelationId = context.InvocationId.ToString();

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