using LMS.Application.DOTs.Authentication;
using LMS.Application.Events;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAzureQueueService _queueService;

        public AuthService(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IJwtTokenService jwtTokenService,
           IAzureQueueService queueService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _queueService = queueService;
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User already exists."
                };
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = string.Join(", ", result.Errors.Select(x => x.Description))
                };
            }

            await _queueService.EnqueueAsync(new UserRegisteredEvent
            {
                Email = user.Email,
                UserId = user.Id,
                RegistrationOn = DateTime.UtcNow
            });

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "User registered successfully."
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid Email or Password"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                false);

            if (!result.Succeeded)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid Email or Password"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenService.GenerateToken(user, roles);

            return new LoginResponseDto
            {
                IsSuccess = true,
                Message = "Login Successful",
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            };
        }
    }
}
