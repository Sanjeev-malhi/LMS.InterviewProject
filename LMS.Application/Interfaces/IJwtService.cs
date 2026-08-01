using LMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Application.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);

        string GenerateRefreshToken();
    }
}
