using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.DOTs.Authentication
{
    public class AuthenticationResponse
    {
        public bool IsSuccess { get; set; }

        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
