# Community Car API - Reference Implementation (RF)

> 🎓 **DEMO/LEARNING REPOSITORY** - Reference implementation for learning Clean Architecture and CQRS patterns

⚠️ **This is a demo version for educational purposes. Not intended for production use.**

## Overview

This repository contains a complete, working implementation of a car-sharing API built with Clean Architecture principles. Use it to learn, study patterns, or as a template for your own projects.

## Features

- Car Management (CRUD)
- User Authentication & Authorization
- Booking System
- Review & Rating System
- Community Q&A Forum
- Admin Dashboard
- RESTful API with Swagger

## Quick Start

```bash
# Restore dependencies
dotnet restore

# Update database
dotnet ef database update --project src/CommunityCarApi.Infrastructure --startup-project src/CommunityCarApi.WebApi

# Run
dotnet run --project src/CommunityCarApi.WebApi
```

Access at: https://localhost:5075

## Architecture

- **Domain**: Entities, Enums, Core logic
- **Application**: Commands, Queries, DTOs, Validators
- **Infrastructure**: Database, Identity, Services
- **WebApi**: Controllers, Middleware

## Technologies

- .NET 9.0
- Entity Framework Core
- ASP.NET Core Identity
- MediatR (CQRS)
- FluentValidation
- Swagger/OpenAPI
- SQL Server
- Docker

## Documentation

- [Implementation Guide](IMPLEMENTATION_GUIDE.md)
- [Project Info](docs/PROJECT_INFO.md)
- [Features](docs/FEATURES.md)
- [Setup Guide](docs/PROJECT_SETUP.md)

## Original Repository

Full version: https://github.com/Mostafa-SAID7/community-car-api

## License

MIT License
