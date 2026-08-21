using LMS.Application.Interfaces;
using LMS.Functions.Services;
using LMS.Infrastructure;
using LMS.Infrastructure.Repositiories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddScoped<IUserRegistrationProcessor, UserRegistrationProcessor>();
builder.Services.AddScoped<IEmailService, MockEmailService>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IPaymentEventProcessor, PaymentEventProcessor>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Build().Run();
