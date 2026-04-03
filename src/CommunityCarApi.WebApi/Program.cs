using CommunityCarApi.Application;
using CommunityCarApi.Infrastructure;
using CommunityCarApi.Infrastructure.Data.Seeders;
using CommunityCarApi.WebApi.Configuration;
using CommunityCarApi.WebApi.HealthChecks;
using CommunityCarApi.WebApi.Middleware;
using Hangfire;
using Serilog;

// Configure Serilog
LoggingConfiguration.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add Authentication & Authorization
builder.Services.AddAuthenticationConfiguration(builder.Configuration);

// Add Swagger
builder.Services.AddSwaggerConfiguration();

// Add CORS
builder.Services.AddCorsConfiguration(builder.Configuration);

// Add Rate Limiting
builder.Services.AddRateLimitingConfiguration(builder.Configuration);

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<RedisHealthCheck>("redis");

var app = builder.Build();

// Configure the HTTP request pipeline

// Add custom middleware
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<PerformanceMonitoringMiddleware>();

app.UseSwaggerConfiguration(app.Environment);

app.UseHttpsRedirection();

// Use HSTS in production
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

// Enable static files (HTML, CSS, images)
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "Home.html" }
});
app.UseStaticFiles();

// Handle 404 errors - redirect to 404.html
app.UseStatusCodePagesWithReExecute("/404.html");

// Use Rate Limiting
app.UseRateLimitingConfiguration();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.MapControllers();

// Map Health Checks
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});

// Fallback route for SPA-style routing - serve 404.html for unmatched routes
app.MapFallbackToFile("404.html");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<DatabaseSeeder>>();
    var seeder = new DatabaseSeeder(services, logger);
    await seeder.SeedAsync();
}

app.Run();
