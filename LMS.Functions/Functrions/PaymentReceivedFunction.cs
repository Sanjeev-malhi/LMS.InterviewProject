using LMS.Application.Events;
using LMS.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Functions.Functrions
{
    public class PaymentReceivedFunction
    {
        private readonly ILogger<PaymentReceivedFunction> _logger;
        private readonly IPaymentEventProcessor _processor;

        public PaymentReceivedFunction(ILogger<PaymentReceivedFunction> logger, IPaymentEventProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }

        [Function(nameof(PaymentReceivedFunction))]
        public async Task Run(
        [QueueTrigger("payment-events", Connection = "AzureWebJobsStorage")]
        PaymentReceivedEvent message,
        FunctionContext context,
        CancellationToken cancellationToken)
        {
            using(_logger.BeginScope(new Dictionary<string, object> {
                ["CorrelationId"] = message.CorrelationId,
                ["EventId"] = message.EventId
            }))
            {
                await _processor.ProcessAsync(message, cancellationToken);
            }
        }
    }
}
