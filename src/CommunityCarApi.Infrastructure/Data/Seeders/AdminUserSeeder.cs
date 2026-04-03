using CommunityCarApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CommunityCarApi.Infrastructure.Data.Seeders;

public class AdminUserSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AdminUserSeeder> _logger;

    public AdminUserSeeder(
        UserManager<ApplicationUser> userManager, 
        IConfiguration configuration,
        ILogger<AdminUserSeeder> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            var adminEmail = _configuration["AdminUser:Email"] ?? "admin@communitycar.com";
            var adminPassword = _configuration["AdminUser:Password"] ?? "Admin@123456";

            var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Bio = "System Administrator Account"
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    _logger.LogInformation("Admin user created successfully with email: {Email}", adminEmail);
                }
                else
                {
                    _logger.LogError("Failed to create admin user: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                _logger.LogInformation("Admin user already exists");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding admin user");
        }
    }
}
