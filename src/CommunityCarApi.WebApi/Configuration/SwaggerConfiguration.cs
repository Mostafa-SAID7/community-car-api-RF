using Microsoft.OpenApi.Models;

namespace CommunityCarApi.WebApi.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Community Car API",
                Version = "v1",
                Description = @"A comprehensive RESTful API for community car sharing platform

**Quick Links:**
- [Home Page](/) - Application home page
- [API Documentation](/Docs.html) - Detailed API documentation with examples
- [EraSoft Website](https://www.eraasoft.com) - Learn more about EraSoft

**Features:**
- Authentication & Authorization with JWT
- Car Management with advanced search
- Booking System with conflict detection
- Community Features (Q&A, Posts, Events)
- Admin Dashboard & Analytics"
            });

            // Add JWT Authentication to Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}
