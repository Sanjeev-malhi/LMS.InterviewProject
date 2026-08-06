using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Events
{
    public class UserRegisteredEvent
    {
        public string UserId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime RegistrationOn { get; set; }
    }
}
