using LMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Domain.Entities
{
    public class Enrollment
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid CourseId { get; set; }

        public string TransactionId { get; set; } = string.Empty;

        public decimal AmountPaid { get; set; }

        public EnrollmentStatus Status { get; set; }

        public int RetryCount { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifiedOn { get; set; }

        public Guid EventId { get; set; }

        public string CorrelationId { get; set; } = string.Empty;
    }
}
