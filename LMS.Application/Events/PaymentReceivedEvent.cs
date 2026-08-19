using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Events
{
    public class PaymentReceivedEvent
    {
        public Guid EventId { get; set; }

        public Guid CourseId { get; set; }

        public Guid UserId { get; set; }

        public string TransactionId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTimeOffset PaymentDate { get; set; }

    }
}
