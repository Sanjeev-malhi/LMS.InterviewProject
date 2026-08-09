using LMS.Application.Configuration;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.Persistence;
using LMS.Infrastructure.Repositiories;
using LMS.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.Configure<AzureStorageSettings>(
        configuration.GetSection("AzureStorage"));

        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        services.AddScoped<IAzureQueueService, AzureQueueService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IEmailHistoryRepository, EmailHistoryRepository>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:ConnectionString"];
            options.InstanceName = "LMS";
        });

        services.AddScoped<ICacheService, RedisCacheService>();


        return services;
    }
}