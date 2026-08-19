using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Events
{
    public class UserRegisteredEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid();

        public string CorrelationId { get; set; }

        public Guid UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public DateTime RegistrationOn { get; set; }
    }
}
