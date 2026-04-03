# Setup Guide - Community Car API RF

## Quick Setup

### Prerequisites

- .NET 9.0 SDK
- SQL Server (any edition: Express, Developer, or full)
- Visual Studio 2022 or VS Code (optional)
- Git

### Database Setup

This demo uses a local SQL Server instance with Windows Authentication.

**Connection String:**
```
Server=.;Database=CommunityCarDbRF;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;
```

**Database Name:** `CommunityCarDbRF`

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/Mostafa-SAID7/community-car-api-RF.git
   cd community-car-api-RF
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Create the database**
   ```bash
   dotnet ef database update --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi
   ```

4. **Run the application**
   ```bash
   dotnet run --project src/CommunityCarApi.WebApi
   ```

5. **Access the application**
   - Home: https://localhost:5075
   - Swagger UI: https://localhost:5075/swagger
   - API Docs: https://localhost:5075/Docs.html

### Default Admin Credentials

- **Email:** admin@demo.com
- **Password:** Demo@123456

⚠️ **Change these credentials in production!**

## Configuration Files

All configuration is in `src/CommunityCarApi.WebApi/`:

- `appsettings.json` - Main configuration
- `appsettings.Development.json` - Development overrides
- `appsettings.Production.json` - Production settings

## Database Migrations

### Create a new migration
```bash
dotnet ef migrations add YourMigrationName --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi
```

### Apply migrations
```bash
dotnet ef database update --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi
```

### Remove last migration
```bash
dotnet ef migrations remove --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi
```

## Troubleshooting

### Cannot connect to database

**Error:** "Cannot open database 'CommunityCarDbRF'"

**Solution:** Make sure SQL Server is running and you have permissions. Try:
```bash
# Check SQL Server service
Get-Service MSSQLSERVER

# If stopped, start it
Start-Service MSSQLSERVER
```

### Port already in use

**Error:** "Address already in use"

**Solution:** Change the port in `Properties/launchSettings.json` or stop the process using port 5075.

### Migration fails

**Error:** Migration command fails

**Solution:** 
1. Delete the database: `DROP DATABASE CommunityCarDbRF`
2. Delete migrations folder: `src/CommunityCarApi.Infrastructure/Migrations`
3. Recreate migration: `dotnet ef migrations add InitialCreate`
4. Apply: `dotnet ef database update`

## Docker Setup (Optional)

```bash
# Build and run with Docker Compose
docker-compose up --build

# Access at http://localhost:8080
```

## Environment Variables

Copy `.env.example` to `.env` and update values:

```env
DB_SERVER=.
DB_NAME=CommunityCarDbRF
JWT_SECRET_KEY=YourSecretKeyHere
ADMIN_EMAIL=admin@demo.com
ADMIN_PASSWORD=Demo@123456
```

## Features to Test

1. **Authentication**
   - Register a new user
   - Login with credentials
   - Get JWT token

2. **Cars**
   - Create a car listing
   - Search cars by location
   - Update car details
   - Delete a car

3. **Bookings**
   - Create a booking
   - View bookings
   - Cancel a booking

4. **Reviews**
   - Add a review for a car
   - View reviews
   - Calculate ratings

5. **Community Q&A**
   - Ask a question
   - Answer questions
   - Vote on questions/answers
   - View leaderboard

## API Testing

Use Swagger UI at https://localhost:5075/swagger to test all endpoints.

### Example API Calls

**Register:**
```bash
POST /api/auth/register
{
  "email": "user@example.com",
  "password": "Password@123",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Login:**
```bash
POST /api/auth/login
{
  "email": "user@example.com",
  "password": "Password@123"
}
```

**Get Cars:**
```bash
GET /api/cars?city=Cairo&isAvailable=true
```

## Next Steps

1. Explore the codebase structure
2. Review the Clean Architecture implementation
3. Study the CQRS pattern with MediatR
4. Modify handlers to add your business logic
5. Add custom features
6. Deploy to your environment

## Support

- Documentation: [README.md](README.md)
- Implementation Guide: [IMPLEMENTATION_GUIDE.md](IMPLEMENTATION_GUIDE.md)
- Detailed Info: [README_RF.md](README_RF.md)
- Original Repo: https://github.com/Mostafa-SAID7/community-car-api

## License

MIT License - Free to use for learning and projects
