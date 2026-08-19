using LMS.Domain.Enums;

namespace LMS.Domain.Entities;

public class EmailHistory
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public EmailType EmailType { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public EmailStatus Status { get; set; }

    public int RetryCount { get; set; }

    public string? ProviderMessageId { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? SentOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public Guid EventId { get; set; }

    public string CorrelationId { get; set; } = string.Empty;
}