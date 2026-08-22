using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Domain.Entities
{
    public class FailedEvents
    {
        public Guid Id { get; set; }

        public string QueueName { get; set; } = string.Empty;

        public string MessageContent { get; set; } = string.Empty;

        public string? CorrelationId { get; set; }

        public DateTime FailedOn { get; set; } = DateTime.UtcNow;

        public bool Reviewed { get; set; }
    }
}
