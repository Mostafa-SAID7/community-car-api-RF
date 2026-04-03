# Community Car API - Deployment Guide

## Prerequisites

- Docker and Docker Compose installed
- .NET 9.0 SDK (for local development)
- SQL Server 2022 or later
- Redis (optional, will fallback to in-memory cache)

## Environment Variables

### Required Environment Variables

```bash
# Database
ConnectionStrings__DefaultConnection="Server=your-server;Database=CommunityCarDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"

# Redis (optional)
ConnectionStrings__Redis="your-redis-server:6379"

# JWT Settings
JwtSettings__SecretKey="YourSuperSecretKeyThatIsAtLeast32CharactersLong!"
JwtSettings__Issuer="CommunityCarApi"
JwtSettings__Audience="CommunityCarClient"
JwtSettings__ExpirationMinutes="60"
JwtSettings__RefreshTokenExpirationDays="7"

# Email Settings
EmailSettings__SmtpServer="smtp.gmail.com"
EmailSettings__Port="587"
EmailSettings__Username="your-email@gmail.com"
EmailSettings__Password="your-app-password"
EmailSettings__From="noreply@communitycar.com"
EmailSettings__EnableSsl="true"

# Admin User
AdminUser__Email="admin@communitycar.com"
AdminUser__Password="Admin@SecurePassword123!"
```

## Deployment Options

### Option 1: Docker Compose (Recommended)

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-repo/community-car-api.git
   cd community-car-api
   ```

2. **Update environment variables in docker-compose.yml**
   - Edit `docker-compose.yml` and update the environment variables
   - Change SQL Server SA password
   - Update JWT secret key
   - Configure email settings

3. **Run with Docker Compose**
   ```bash
   docker-compose up -d
   ```

4. **Check logs**
   ```bash
   docker-compose logs -f api
   ```

5. **Access the API**
   - API: http://localhost:5075
   - Swagger: http://localhost:5075/swagger
   - Health Check: http://localhost:5075/health

### Option 2: Manual Deployment

1. **Setup Database**
   ```bash
   # Create database
   sqlcmd -S localhost -U sa -P YourPassword -Q "CREATE DATABASE CommunityCarDb"
   ```

2. **Update appsettings.Production.json**
   - Configure connection strings
   - Set JWT secret key
   - Configure email settings
   - Set allowed origins

3. **Run Migrations**
   ```bash
   cd src/CommunityCarApi.WebApi
   dotnet ef database update --project ../CommunityCarApi.Infrastructure
   ```

4. **Publish the Application**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

5. **Run the Application**
   ```bash
   cd publish
   dotnet CommunityCarApi.WebApi.dll
   ```

### Option 3: Azure App Service

1. **Create Azure Resources**
   - Azure App Service (Linux, .NET 9.0)
   - Azure SQL Database
   - Azure Cache for Redis (optional)

2. **Configure App Settings**
   - Add all environment variables in Azure Portal
   - Configure connection strings
   - Set deployment source (GitHub Actions)

3. **Deploy using GitHub Actions**
   - The `.github/workflows/dotnet-ci.yml` workflow is already configured
   - Add Azure publish profile as GitHub secret
   - Push to main branch to trigger deployment

## Post-Deployment Steps

### 1. Verify Health Checks

```bash
curl http://your-domain/health
```

Expected response:
```json
{
  "status": "Healthy",
  "checks": {
    "database": "Healthy",
    "redis": "Healthy"
  }
}
```

### 2. Create Admin User

The admin user is automatically created on first startup with credentials from `AdminUser` configuration.

Default credentials (CHANGE IMMEDIATELY):
- Email: admin@communitycar.com
- Password: Admin@123456

### 3. Test Authentication

```bash
# Login
curl -X POST http://your-domain/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@communitycar.com","password":"Admin@123456"}'
```

### 4. Access Swagger Documentation

Navigate to: `http://your-domain/swagger`

### 5. Monitor Hangfire Dashboard

Navigate to: `http://your-domain/hangfire`
(Requires Admin role)

## Security Checklist

- [ ] Change default admin password
- [ ] Update JWT secret key with a strong random value
- [ ] Configure HTTPS/SSL certificates
- [ ] Set up firewall rules
- [ ] Configure CORS for production domains only
- [ ] Enable rate limiting
- [ ] Set up monitoring and logging
- [ ] Configure backup strategy for database
- [ ] Review and update security headers
- [ ] Enable SQL Server encryption
- [ ] Use Azure Key Vault or similar for secrets

## Monitoring

### Application Logs

Logs are written to:
- Console (Docker logs)
- File: `logs/log-{Date}.txt`

View logs:
```bash
# Docker
docker-compose logs -f api

# Local
tail -f logs/log-$(date +%Y%m%d).txt
```

### Health Endpoints

- `/health` - Overall health status
- `/health/ready` - Readiness probe
- `/health/live` - Liveness probe

### Hangfire Dashboard

Monitor background jobs at `/hangfire`:
- Job execution history
- Recurring jobs status
- Failed jobs
- Server statistics

## Troubleshooting

### Database Connection Issues

```bash
# Test SQL Server connection
sqlcmd -S your-server -U sa -P YourPassword -Q "SELECT 1"

# Check if database exists
sqlcmd -S your-server -U sa -P YourPassword -Q "SELECT name FROM sys.databases WHERE name = 'CommunityCarDb'"
```

### Redis Connection Issues

```bash
# Test Redis connection
redis-cli -h your-redis-server ping

# Check Redis info
redis-cli -h your-redis-server info
```

### Application Won't Start

1. Check logs for errors
2. Verify all environment variables are set
3. Ensure database is accessible
4. Check port availability
5. Verify .NET 9.0 runtime is installed

## Scaling

### Horizontal Scaling

The application is stateless and can be scaled horizontally:

```yaml
# docker-compose.yml
services:
  api:
    deploy:
      replicas: 3
```

### Load Balancing

Use a reverse proxy (Nginx, Azure Load Balancer, etc.):

```nginx
upstream api_backend {
    server api1:80;
    server api2:80;
    server api3:80;
}

server {
    listen 80;
    location / {
        proxy_pass http://api_backend;
    }
}
```

## Backup Strategy

### Database Backup

```bash
# SQL Server backup
sqlcmd -S your-server -U sa -P YourPassword -Q "BACKUP DATABASE CommunityCarDb TO DISK = '/var/opt/mssql/backup/CommunityCarDb.bak'"
```

### Redis Backup

Redis automatically creates RDB snapshots in `/data` volume.

## Support

For issues and questions:
- GitHub Issues: https://github.com/your-repo/community-car-api/issues
- Documentation: https://your-domain/docs
- API Reference: https://your-domain/swagger
