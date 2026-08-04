using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.DOTs.Authentication
{
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }
    }
}
