using LMS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LMS.Infrastructure.Services
{
    public class HmacWebhookSignatureValidator : IWebhookSignatureValidator
    {
        public bool IsValid(string rawPayload, string receivedSignature, string secret)
        {
            if(string.IsNullOrEmpty(rawPayload) || string.IsNullOrEmpty(receivedSignature))
            {
                return false;
            }

            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var payloadBytes = Encoding.UTF8.GetBytes(rawPayload);

            using var hmac = new HMACSHA256(keyBytes);
            var computedHash = hmac.ComputeHash(payloadBytes);
            var computedSignature = Convert.ToHexString(computedHash).ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedSignature),
                Encoding.UTF8.GetBytes(receivedSignature.ToLowerInvariant()));
        }
    }
}
