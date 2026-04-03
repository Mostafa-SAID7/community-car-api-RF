# Community Car API - RF (Reference) Version

## What is This?

This is the **Reference Implementation (RF)** version of the Community Car API. It's a complete, working copy of the main repository that serves as:

- **Learning Resource**: Study Clean Architecture and CQRS patterns
- **Project Template**: Start your own car-sharing or similar platform
- **Code Reference**: See how features are implemented
- **Demo Version**: Show the architecture without production concerns

## Differences from Main Repository

| Aspect | Main Repo | RF Version |
|--------|-----------|------------|
| Purpose | Production-ready | Learning/Template |
| Business Logic | Complete | Complete (for reference) |
| Documentation | Production-focused | Learning-focused |
| Git History | Full history | Fresh start |
| Deployment | Production configs | Demo configs |

## What's Included

✅ All source code and implementations
✅ Complete Clean Architecture structure
✅ Working CQRS pattern with MediatR
✅ Entity Framework Core with migrations
✅ ASP.NET Core Identity setup
✅ FluentValidation examples
✅ Swagger/OpenAPI documentation
✅ Docker configuration
✅ CI/CD workflows
✅ Static frontend pages

## Quick Start

```bash
# 1. Restore packages
dotnet restore

# 2. Setup database
dotnet ef database update --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi

# 3. Run the API
dotnet run --project src/CommunityCarApi.WebApi
```

Access the application:
- **Home**: https://localhost:5075
- **Swagger UI**: https://localhost:5075/swagger
- **API Docs**: https://localhost:5075/Docs.html

## Use Cases

### 1. Learning Clean Architecture
Study how the layers interact:
- Domain → Core business entities
- Application → Use cases and business logic
- Infrastructure → Data access and services
- WebApi → Controllers and configuration

### 2. Understanding CQRS
See how commands and queries are separated:
- Commands in `Features/*/Commands/`
- Queries in `Features/*/Queries/`
- Handlers implement business logic
- MediatR coordinates the flow

### 3. Starting Your Own Project
Use as a template:
1. Clone this repository
2. Rename namespaces and projects
3. Modify entities for your domain
4. Implement your business logic
5. Deploy your application

### 4. Code Reference
Look up implementations:
- How to structure a feature
- How to implement validation
- How to handle authentication
- How to organize database context

## Project Structure

```
community-car-api-RF/
├── src/
│   ├── CommunityCarApi.Domain/
│   │   ├── Entities/          # Car, Booking, Review, etc.
│   │   ├── Enums/             # CarType, BookingStatus, etc.
│   │   └── Common/            # BaseEntity, interfaces
│   │
│   ├── CommunityCarApi.Application/
│   │   ├── Features/          # CQRS Commands & Queries
│   │   │   ├── Cars/
│   │   │   ├── Bookings/
│   │   │   ├── Auth/
│   │   │   └── Reviews/
│   │   ├── DTOs/              # Data Transfer Objects
│   │   └── Common/            # Result pattern, interfaces
│   │
│   ├── CommunityCarApi.Infrastructure/
│   │   ├── Data/              # DbContext, migrations
│   │   ├── Identity/          # User management
│   │   └── Services/          # External services
│   │
│   └── CommunityCarApi.WebApi/
│       ├── Controllers/       # API endpoints
│       ├── Middleware/        # Custom middleware
│       └── wwwroot/           # Static files
│
├── docs/                      # Documentation
├── .github/workflows/         # CI/CD pipelines
├── docker-compose.yml         # Docker setup
└── README.md                  # This file
```

## Key Features Implemented

### Cars Module
- Create, Read, Update, Delete cars
- Search and filter by location, type, price
- Image upload support
- Availability management

### Authentication
- User registration
- Login with JWT tokens
- Role-based authorization
- Password management

### Bookings
- Create bookings with date validation
- Cancel bookings
- View booking history
- Booking status management

### Reviews
- Rate and review cars
- View reviews by car
- Calculate average ratings
- Review statistics

### Community Q&A
- Ask questions
- Answer questions
- Vote on questions/answers
- Reputation system
- Leaderboard

### Admin Features
- User management
- Dashboard statistics
- Role assignment
- System monitoring

## Technologies Used

- **.NET 9.0**: Latest .NET framework
- **Entity Framework Core**: ORM for database access
- **ASP.NET Core Identity**: Authentication and authorization
- **MediatR**: CQRS pattern implementation
- **FluentValidation**: Input validation
- **AutoMapper**: Object mapping
- **Swagger/OpenAPI**: API documentation
- **SQL Server**: Database
- **Docker**: Containerization

## Documentation

- **[IMPLEMENTATION_GUIDE.md](IMPLEMENTATION_GUIDE.md)**: How to customize and extend
- **[docs/PROJECT_INFO.md](docs/PROJECT_INFO.md)**: Detailed project information
- **[docs/FEATURES.md](docs/FEATURES.md)**: Feature specifications
- **[docs/TECHNOLOGIES.md](docs/TECHNOLOGIES.md)**: Technology stack details
- **[docs/PROJECT_SETUP.md](docs/PROJECT_SETUP.md)**: Development environment setup
- **[docs/API_REFERENCE.md](docs/API_REFERENCE.md)**: API endpoint documentation

## Customization Tips

### Modify for Your Domain

1. **Rename Projects**: Update namespaces to match your project
2. **Change Entities**: Modify domain entities for your needs
3. **Update DTOs**: Adjust data transfer objects
4. **Implement Logic**: Add your business rules in handlers
5. **Customize UI**: Update static pages and branding

### Add New Features

1. Create feature folder in `Application/Features/`
2. Add Command/Query classes
3. Implement Handler classes
4. Create Validator classes
5. Add Controller endpoints
6. Update database with migrations

### Deploy

```bash
# Using Docker
docker-compose up --build

# Or publish to a server
dotnet publish -c Release
```

## Learning Path

1. **Start with Domain Layer**: Understand entities and enums
2. **Study Application Layer**: See how CQRS works
3. **Explore Infrastructure**: Learn data access patterns
4. **Review WebApi Layer**: Understand API structure
5. **Run and Test**: Use Swagger to test endpoints
6. **Modify and Experiment**: Make changes and see results

## Common Tasks

### Add a Database Migration

```bash
dotnet ef migrations add YourMigrationName \
  --project src/CommunityCarApi.Infrastructure \
  --startup-project src/CommunityCarApi.WebApi
```

### Update Database

```bash
dotnet ef database update \
  --project src/CommunityCarApi.Infrastructure \
  --startup-project src/CommunityCarApi.WebApi
```

### Run with Docker

```bash
docker-compose up --build
```

### Run Tests (if you add them)

```bash
dotnet test
```

## Original Repository

This is a reference copy of:
**https://github.com/Mostafa-SAID7/community-car-api**

For the latest production version, visit the main repository.

## License

MIT License - Free to use as a template for your projects

## Contributing

This is a reference implementation. For contributions to the main project, visit the original repository.

## Support

- Check the [Implementation Guide](IMPLEMENTATION_GUIDE.md)
- Review the [documentation](docs/)
- Visit the [original repository](https://github.com/Mostafa-SAID7/community-car-api)

---

**Created as a learning resource and project template**

Happy coding! 🚀
