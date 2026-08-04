using LMS.Application.DOTs.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

public interface IAuthService
{
    Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);

    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
