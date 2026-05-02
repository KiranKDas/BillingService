using BillingService.Data;
using BillingService.Business;
using Microsoft.EntityFrameworkCore;
using BillingService.Swagger;
using Serilog;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;
using OpenTelemetry;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

builder.Services.AddDbContext<BillingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BillingDb"), 
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, 
            maxRetryDelay: TimeSpan.FromSeconds(10), 
            errorCodesToAdd: null)));
builder.Services.AddScoped<IBusiness, Business>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddOperationTransformer<UserTypeHeaderFilter>();
});

builder.Services.AddSingleton<BillingService.Monitoring.BillingMetrics>();
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddMeter(BillingService.Monitoring.BillingMetrics.MeterName);
        metrics.AddPrometheusExporter();
    });

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseSerilogRequestLogging();

app.UseAuthorization();
app.MapControllers();
app.MapPrometheusScrapingEndpoint();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BillingDbContext>();
    context.Database.Migrate();
}

app.Run();