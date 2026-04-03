# Project Setup Guide

This guide provides detailed instructions for setting up the Community Car API project.

## Prerequisites

- .NET SDK 9.0 or higher
- SQL Server 2022 or SQL Server Express
- Redis (optional - falls back to memory cache)
- Visual Studio 2022, VS Code, or JetBrains Rider
- Git

## Quick Start

### 1. Clone the Repository
```bash
git clone https://github.com/Mostafa-SAID7/community-car-api.git
cd community-car-api
```

### 2. Configure Database Connection

Edit `src/CommunityCarApi.WebApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CommunityCarDb;Trusted_Connection=True;TrustServerCertificate=True;",
    "Redis": "localhost:6379"
  }
}
```

### 3. Run Database Migrations
```bash
dotnet ef database update --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi
```

### 4. Run the Application
```bash
dotnet restore
dotnet run --project src/CommunityCarApi.WebApi
```

The API will be available at:
- **HTTP**: http://localhost:5075
- **HTTPS**: https://localhost:7075
- **Swagger UI**: http://localhost:5075/swagger
- **API Documentation**: http://localhost:5075/Docs.html
- **Hangfire Dashboard**: http://localhost:5075/hangfire (Admin only)

## Default Admin Account

The application automatically seeds a default admin account on first run:
- **Email**: admin@communitycar.com
- **Password**: Admin@123456
- **Role**: Admin

## Configuration Options

### JWT Settings
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "CommunityCarApi",
    "Audience": "CommunityCarClient",
    "ExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Email Settings
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "noreply@communitycar.com",
    "SenderName": "Community Car",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

### Rate Limiting
```json
{
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 60
      }
    ]
  }
}
```

## Environment Variables

For production, use environment variables instead of appsettings.json:

```bash
ConnectionStrings__DefaultConnection="Server=..."
ConnectionStrings__Redis="localhost:6379"
JwtSettings__SecretKey="..."
EmailSettings__Username="..."
EmailSettings__Password="..."
```

## Development Tools

### Entity Framework Core Tools

Install EF Core tools globally:
```bash
dotnet tool install --global dotnet-ef
```

### Create Migration
```bash
dotnet ef migrations add MigrationName --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi
```

### Update Database
```bash
dotnet ef database update --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi
```

### Remove Last Migration
```bash
dotnet ef migrations remove --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi
```

### Run with Hot Reload
```bash
dotnet watch run --project src/CommunityCarApi.WebApi
```

## Docker Setup

### Using Docker Compose (Recommended)
```bash
docker-compose up -d
```

This starts:
- SQL Server 2022 (port 1433)
- Redis 7 (port 6379)
- Community Car API (port 5075)

### Build Docker Image
```bash
docker build -t community-car-api .
```

### Run Docker Container
```bash
docker run -d -p 5075:80 --name community-car-api community-car-api
```

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string format
- Ensure database exists or migrations have run
- Check firewall settings

### Migration Issues
- Ensure EF Core tools are installed: `dotnet tool install --global dotnet-ef`
- Delete Migrations folder and recreate if corrupted
- Check that Infrastructure project references are correct

### Port Already in Use
- Change ports in `launchSettings.json`
- Kill process using the port: `netstat -ano | findstr :5075`
- Use different ports in appsettings.json

### Redis Connection Issues
- Verify Redis is running
- Application will fall back to memory cache if Redis is unavailable
- Check Redis connection string in appsettings.json

### Hangfire Dashboard Not Accessible
- Ensure you're logged in as Admin
- Check Hangfire configuration in Program.cs
- Verify SQL Server connection for Hangfire storage

## IDE Setup

### Visual Studio 2022
1. Open `CommunityCarApi.sln`
2. Set `CommunityCarApi.WebApi` as startup project
3. Press F5 to run

### VS Code
1. Open project folder
2. Install C# extension
3. Press F5 or use terminal: `dotnet run --project src/CommunityCarApi.WebApi`

### JetBrains Rider
1. Open `CommunityCarApi.sln`
2. Set `CommunityCarApi.WebApi` as startup project
3. Click Run button

## Next Steps

After setup:
1. Visit http://localhost:5075/swagger to explore the API
2. Login with default admin account
3. Review API documentation at http://localhost:5075/Docs.html
4. Check Hangfire dashboard at http://localhost:5075/hangfire
5. See [API_REFERENCE.md](API_REFERENCE.md) for endpoint details
6. See [USE_CASES.md](USE_CASES.md) for usage examples
