using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);
Console.WriteLine("====================================");
Console.WriteLine(builder.Configuration["AzureWebJobsStorage"]);
Console.WriteLine("====================================");
builder.Configuration.AddUserSecrets<Program>();
builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();


builder.Build().Run();
