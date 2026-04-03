using CommunityCarApi.Application.Common.Configuration;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Domain.Entities.Identity;
using CommunityCarApi.Infrastructure.Data;
using CommunityCarApi.Infrastructure.Repositories;
using CommunityCarApi.Infrastructure.Services;
using CommunityCarApi.Infrastructure.Services.Auth;
using CommunityCarApi.Infrastructure.Services.Background;
using CommunityCarApi.Infrastructure.Services.Background.Jobs;
using CommunityCarApi.Infrastructure.Services.Caching;
using CommunityCarApi.Infrastructure.Services.Email;
using CommunityCarApi.Infrastructure.Services.Identity;
using CommunityCarApi.Infrastructure.UnitOfWork;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CommunityCarApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false; // Set to true in production
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // JWT Settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IGamificationService, GamificationService>();

        // Caching - Use Memory Cache by default, Redis if configured
        services.AddMemoryCache();
        var redisConnection = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnection))
        {
            try
            {
                var redis = ConnectionMultiplexer.Connect($"{redisConnection},abortConnect=false");
                services.AddSingleton<IConnectionMultiplexer>(redis);
                services.AddSingleton<ICacheService, RedisCacheService>();
            }
            catch
            {
                // Fallback to memory cache if Redis connection fails
                services.AddSingleton<ICacheService, MemoryCacheService>();
            }
        }
        else
        {
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }

        // Background Jobs
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        services.AddScoped<UpdateBookingStatusJob>();
        services.AddScoped<CleanupExpiredTokensJob>();

        // Hangfire
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();

        // HttpContextAccessor
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}
