using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Interfaces
{
    public interface IWebhookSignatureValidator
    {
        bool IsValid(string rawPayload, string receivedSignature, string secret);
    }
}
