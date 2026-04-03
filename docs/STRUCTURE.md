# Project Structure

This document describes the organization and architecture of the Community Car API project.

## Overview

The Community Car API follows Clean Architecture principles with clear separation of concerns across four distinct layers.

## Directory Structure

```
community-car-api/
├── src/
│   ├── CommunityCarApi.Domain/          # Core business entities
│   │   ├── Entities/                    # Domain entities
│   │   │   ├── ApplicationUser.cs       # User entity
│   │   │   ├── Car.cs                   # Car entity
│   │   │   ├── Booking.cs               # Booking entity
│   │   │   ├── Review.cs                # Review entity
│   │   │   ├── RefreshToken.cs          # Refresh token entity
│   │   │   └── Community/               # Community entities
│   │   │       ├── Question.cs
│   │   │       ├── Answer.cs
│   │   │       ├── QuestionVote.cs
│   │   │       ├── AnswerVote.cs
│   │   │       ├── UserReputation.cs
│   │   │       ├── Badge.cs
│   │   │       └── UserBadge.cs
│   │   ├── Enums/                       # Domain enumerations
│   │   │   ├── BookingStatus.cs
│   │   │   ├── CarType.cs
│   │   │   ├── FuelType.cs
│   │   │   ├── TransmissionType.cs
│   │   │   ├── QuestionCategory.cs
│   │   │   ├── VoteType.cs
│   │   │   └── BadgeType.cs
│   │   └── Common/                      # Shared domain logic
│   │       └── BaseEntity.cs
│   │
│   ├── CommunityCarApi.Application/     # Use cases and business logic
│   │   ├── Features/                    # Feature-based organization (CQRS)
│   │   │   ├── Auth/                    # Authentication
│   │   │   │   ├── Commands/            # Login, Register
│   │   │   │   └── Validators/
│   │   │   ├── Cars/                    # Car management
│   │   │   │   ├── Commands/            # Create, Update, Delete
│   │   │   │   ├── Queries/             # GetCars, GetCarById
│   │   │   │   └── Validators/
│   │   │   ├── Bookings/                # Booking operations
│   │   │   │   ├── Commands/            # Create, Cancel
│   │   │   │   ├── Queries/             # GetBookings, GetBookingById
│   │   │   │   └── Validators/
│   │   │   ├── Users/                   # User management
│   │   │   │   ├── Commands/            # UpdateProfile, ChangePassword
│   │   │   │   └── Queries/             # GetProfile, GetStatistics
│   │   │   ├── Reviews/                 # Review system
│   │   │   │   ├── Commands/            # Create, Update, Delete
│   │   │   │   ├── Queries/             # GetReviews, GetStatistics
│   │   │   │   └── Validators/
│   │   │   ├── Community/               # Community Q&A
│   │   │   │   └── QA/
│   │   │   │       ├── Commands/        # Ask, Answer, Vote, Accept
│   │   │   │       └── Queries/         # GetQuestions, Leaderboard
│   │   │   └── Admin/                   # Admin operations
│   │   │       ├── Dashboard/
│   │   │       │   └── Queries/         # Statistics, Metrics
│   │   │       └── Users/
│   │   │           ├── Commands/        # AssignRole, RemoveRole
│   │   │           └── Queries/         # GetUsers
│   │   ├── Common/                      # Shared application logic
│   │   │   ├── Behaviors/               # MediatR pipeline behaviors
│   │   │   │   ├── ValidationBehavior.cs
│   │   │   │   ├── LoggingBehavior.cs
│   │   │   │   └── PerformanceBehavior.cs
│   │   │   ├── Configuration/           # Configuration models
│   │   │   │   └── JwtSettings.cs
│   │   │   ├── Interfaces/              # Application interfaces
│   │   │   │   ├── IApplicationDbContext.cs
│   │   │   │   ├── ICurrentUserService.cs
│   │   │   │   ├── IDateTime.cs
│   │   │   │   ├── IEmailService.cs
│   │   │   │   ├── IJwtTokenService.cs
│   │   │   │   ├── ICacheService.cs
│   │   │   │   ├── IBackgroundJobService.cs
│   │   │   │   └── IGamificationService.cs
│   │   │   ├── Models/                  # Shared models
│   │   │   │   └── PaginatedList.cs
│   │   │   ├── Result.cs                # Result pattern
│   │   │   └── ErrorCodes.cs            # Error code constants
│   │   ├── DTOs/                        # Data Transfer Objects
│   │   │   ├── Auth/
│   │   │   ├── Community/
│   │   │   ├── Admin/
│   │   │   ├── CarDto.cs
│   │   │   ├── BookingDto.cs
│   │   │   ├── ReviewDto.cs
│   │   │   └── UserDto.cs
│   │   └── DependencyInjection.cs       # Service registration
│   │
│   ├── CommunityCarApi.Infrastructure/  # External concerns
│   │   ├── Data/                        # Database context
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Configurations/          # EF Core entity configurations
│   │   │   ├── Migrations/              # Database migrations
│   │   │   └── Seeders/                 # Data seeders
│   │   │       ├── RoleSeeder.cs
│   │   │       ├── AdminUserSeeder.cs
│   │   │       ├── BadgeSeeder.cs
│   │   │       └── DatabaseSeeder.cs
│   │   ├── Repositories/                # Repository implementations
│   │   │   ├── IRepository.cs
│   │   │   ├── Repository.cs
│   │   │   ├── ICarRepository.cs
│   │   │   ├── CarRepository.cs
│   │   │   ├── IBookingRepository.cs
│   │   │   └── BookingRepository.cs
│   │   ├── UnitOfWork/                  # Unit of Work pattern
│   │   │   ├── IUnitOfWork.cs
│   │   │   └── UnitOfWork.cs
│   │   ├── Services/                    # Service implementations
│   │   │   ├── DateTimeService.cs
│   │   │   ├── Identity/
│   │   │   │   ├── CurrentUserService.cs
│   │   │   │   └── JwtTokenService.cs
│   │   │   ├── Email/
│   │   │   │   └── EmailService.cs
│   │   │   ├── Caching/
│   │   │   │   ├── RedisCacheService.cs
│   │   │   │   └── MemoryCacheService.cs
│   │   │   ├── Background/
│   │   │   │   ├── BackgroundJobService.cs
│   │   │   │   ├── UpdateBookingStatusJob.cs
│   │   │   │   └── CleanupExpiredTokensJob.cs
│   │   │   └── Gamification/
│   │   │       └── GamificationService.cs
│   │   └── DependencyInjection.cs       # Service registration
│   │
│   └── CommunityCarApi.WebApi/          # API layer
│       ├── Controllers/                 # API controllers
│       │   ├── AuthController.cs
│       │   ├── CarsController.cs
│       │   ├── BookingsController.cs
│       │   ├── UsersController.cs
│       │   ├── ReviewsController.cs
│       │   ├── Community/
│       │   │   └── QAController.cs
│       │   └── Admin/
│       │       ├── AdminDashboardController.cs
│       │       └── AdminUsersController.cs
│       ├── Middleware/                  # Custom middleware
│       │   ├── ExceptionHandlingMiddleware.cs
│       │   ├── RequestLoggingMiddleware.cs
│       │   ├── PerformanceMonitoringMiddleware.cs
│       │   └── SecurityHeadersMiddleware.cs
│       ├── HealthChecks/                # Health check implementations
│       │   ├── DatabaseHealthCheck.cs
│       │   └── RedisHealthCheck.cs
│       ├── Configuration/               # Startup configuration
│       │   ├── SwaggerConfiguration.cs
│       │   ├── AuthenticationConfiguration.cs
│       │   ├── LoggingConfiguration.cs
│       │   ├── CorsConfiguration.cs
│       │   └── RateLimitingConfiguration.cs
│       ├── wwwroot/                     # Static files
│       │   ├── Home.html
│       │   ├── Docs.html
│       │   ├── 404.html
│       │   └── community-car.css
│       ├── appsettings.json             # Configuration
│       ├── appsettings.Development.json
│       ├── appsettings.Production.json
│       └── Program.cs                   # Application entry point
│
├── docs/                                # Documentation
│   ├── API_REFERENCE.md
│   ├── IMPLEMENTATION_PLAN.md
│   ├── DEPLOYMENT.md
│   ├── PROJECT_SETUP.md
│   ├── STRUCTURE.md
│   ├── FEATURES.md
│   ├── TECHNOLOGIES.md
│   ├── USE_CASES.md
│   ├── ERD.md
│   ├── SECURITY.md
│   ├── CHANGELOG.md
│   ├── CONTRIBUTING.md
│   ├── CONTRIBUTORS.md
│   ├── CODE_OF_CONDUCT.md
│   └── PROJECT_INFO.md
│
├── screenshots/                         # Application screenshots
├── .github/workflows/                   # CI/CD workflows
│   ├── dotnet-ci.yml
│   ├── ci-backend.yml
│   ├── codeql-analysis.yml
│   ├── dependency-review.yml
│   └── auto-merge.yml
├── .dockerignore
├── Dockerfile
├── docker-compose.yml
├── .env.example
├── .gitignore
└── README.md
```

## Layer Responsibilities

### Domain Layer (CommunityCarApi.Domain)
- **Purpose**: Core business entities and rules
- **Contains**:
  - Entity classes (Car, Booking, User, etc.)
  - Enumerations (BookingStatus, CarType, etc.)
  - Domain interfaces
  - Business rules and validations
  - Value objects
- **Dependencies**: None (innermost layer)
- **Rules**: No dependencies on other layers or external frameworks

### Application Layer (CommunityCarApi.Application)
- **Purpose**: Use cases and business logic orchestration
- **Contains**:
  - Commands (write operations)
  - Queries (read operations)
  - DTOs (Data Transfer Objects)
  - Application interfaces
  - Validation rules (FluentValidation)
  - MediatR behaviors
- **Dependencies**: Domain layer only
- **Rules**: No dependencies on Infrastructure or WebApi layers

### Infrastructure Layer (CommunityCarApi.Infrastructure)
- **Purpose**: External concerns and implementations
- **Contains**:
  - Database context (EF Core)
  - Repository implementations
  - Service implementations (Email, Caching, etc.)
  - Data seeders
  - External API integrations
  - File system access
- **Dependencies**: Application and Domain layers
- **Rules**: Implements interfaces defined in Application layer

### WebApi Layer (CommunityCarApi.WebApi)
- **Purpose**: HTTP API and presentation
- **Contains**:
  - Controllers (API endpoints)
  - Middleware
  - Configuration
  - Health checks
  - Static files
  - Startup logic
- **Dependencies**: Application and Infrastructure layers
- **Rules**: Orchestrates requests to Application layer via MediatR

## Design Patterns

### CQRS (Command Query Responsibility Segregation)
- **Commands**: Modify state (Create, Update, Delete)
- **Queries**: Read data (Get, List, Search)
- **Implementation**: MediatR library
- **Benefits**: Clear separation, optimized queries, scalability

### Repository Pattern
- **Purpose**: Abstracts data access logic
- **Implementation**: Generic repository + specific repositories
- **Benefits**: Testability, centralized data logic, swappable data sources

### Unit of Work Pattern
- **Purpose**: Manages transactions across repositories
- **Implementation**: UnitOfWork class coordinating repositories
- **Benefits**: Data consistency, transaction management

### Result Pattern
- **Purpose**: Standardized error handling
- **Implementation**: Result<T> class with Success/Failure
- **Benefits**: Type-safe, rich error information, no exceptions for business logic

### Specification Pattern
- **Purpose**: Reusable query logic
- **Implementation**: Specification classes for complex queries
- **Benefits**: Composable, testable, reusable queries

## Naming Conventions

### Files and Folders
- **PascalCase** for all files and folders
- **Suffix with type**: Controller, Service, Repository, Command, Query, etc.
- **Examples**: `CarsController.cs`, `CreateCarCommand.cs`, `GetCarsQuery.cs`

### Namespaces
- Match folder structure exactly
- Use PascalCase
- **Example**: `CommunityCarApi.Application.Features.Cars.Commands`

### Classes
- **PascalCase** for class names
- Descriptive names indicating purpose
- Single Responsibility Principle
- **Examples**: `CreateCarCommandHandler`, `CarRepository`

### Methods
- **PascalCase** for method names
- Verb-based names indicating action
- Clear intent
- **Examples**: `CreateAsync`, `GetByIdAsync`, `UpdateAsync`

### Variables and Parameters
- **camelCase** for local variables and parameters
- **PascalCase** for properties
- Descriptive names
- **Examples**: `carId`, `userId`, `CreatedAt`

### Constants
- **PascalCase** for constants
- Descriptive names
- **Example**: `MaxUploadSizeInBytes`

## Request Flow

1. **HTTP Request** → Controller endpoint
2. **Controller** → Creates Command/Query → Sends to MediatR
3. **MediatR** → Executes pipeline behaviors (Validation, Logging, Performance)
4. **Handler** → Processes Command/Query
5. **Handler** → Uses Repository/Service from Infrastructure
6. **Repository** → Accesses database via EF Core
7. **Handler** → Returns Result<T>
8. **Controller** → Maps Result to HTTP response
9. **HTTP Response** → Returned to client

## Dependency Flow

```
WebApi → Application → Domain
  ↓
Infrastructure → Application → Domain
```

- **WebApi** depends on Application and Infrastructure
- **Infrastructure** depends on Application and Domain
- **Application** depends on Domain only
- **Domain** has no dependencies

## Key Principles

1. **Separation of Concerns**: Each layer has a specific responsibility
2. **Dependency Inversion**: Depend on abstractions, not concretions
3. **Single Responsibility**: Each class has one reason to change
4. **Open/Closed**: Open for extension, closed for modification
5. **Interface Segregation**: Many specific interfaces over one general
6. **DRY (Don't Repeat Yourself)**: Reuse code through abstraction

## Testing Strategy

- **Unit Tests**: Test business logic in Application layer
- **Integration Tests**: Test Infrastructure layer with real database
- **API Tests**: Test WebApi layer endpoints
- **Test Isolation**: Each layer can be tested independently

For more details, see [TECHNOLOGIES.md](TECHNOLOGIES.md) and [FEATURES.md](FEATURES.md).
