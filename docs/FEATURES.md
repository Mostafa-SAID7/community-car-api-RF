# Features

This document provides a comprehensive list of all features implemented in the Community Car API.

## Core Features

### Authentication & Authorization
- JWT Bearer token authentication
- Role-based access control (Admin, User, Moderator)
- Password hashing with ASP.NET Core Identity
- Automatic role seeding on startup
- Secure token generation and validation
- Authorization policies for endpoints

### Car Management
- Complete CRUD operations
- Advanced filtering:
  - City and state
  - Year range
  - Price range
  - Car type (Sedan, SUV, Truck, etc.)
  - Fuel type (Gasoline, Diesel, Electric, Hybrid)
  - Transmission type (Manual, Automatic)
  - Minimum seats
  - Availability status
- Ownership validation
- Admin override capabilities
- Soft delete implementation
- Car availability checking

### Booking System
- Real-time availability checking
- Booking conflict detection
- Automatic booking number generation (BK + timestamp)
- Status tracking:
  - Pending
  - Confirmed
  - Completed
  - Cancelled
- Automatic price calculation based on daily rate
- Date validation:
  - No past dates allowed
  - End date must be after start date
- Ownership validation for cancellation
- Admin override for cancellation
- Business rules enforcement (can't cancel started/completed bookings)
- Payment status tracking

### User Management
- User registration and login
- Profile management (first name, last name, phone number)
- Password change with validation
- User statistics:
  - Total bookings
  - Cars owned
  - Total amount spent
- Role information in profile
- Phone number validation

### Review System
- Car reviews with 1-5 star ratings
- Review verification (must have completed booking to review car)
- Duplicate review prevention (one review per user per car)
- Review statistics:
  - Average rating
  - Total reviews
  - Star distribution (5-star, 4-star, etc.)
- Filtering by:
  - Car
  - User
  - Rating
  - Verification status
- Ownership validation for updates/deletes
- Admin override for deletion
- Soft delete implementation

### Community Q&A System
- Question and answer platform
- 10 question categories:
  - General
  - Maintenance
  - Insurance
  - Safety
  - Fuel Efficiency
  - Buying Guide
  - Selling Tips
  - Legal
  - Technology
  - Modifications
- Voting system (upvote/downvote) for questions and answers
- Answer acceptance by question authors
- Gamification with points, levels, and badges
- User reputation tracking
- Leaderboard with time period filters (all-time, monthly, weekly)
- Trending questions calculation
- View count tracking
- Advanced filtering:
  - Category
  - Search text
  - Tags
  - Solved status
- Sorting options:
  - Date
  - Votes
  - Answers
  - Views
- Soft delete for questions and answers

### Gamification System
- Points system for all actions:
  - Question created: +5 points
  - Answer created: +10 points
  - Question upvoted: +5 points
  - Question downvoted: -2 points
  - Answer upvoted: +10 points
  - Answer downvoted: -5 points
  - Answer accepted: +25 points
- 5 levels with automatic progression:
  - Level 1 (Beginner): 0-99 points
  - Level 2 (Contributor): 100-499 points
  - Level 3 (Expert): 500-999 points
  - Level 4 (Master): 1000-2499 points
  - Level 5 (Legend): 2500+ points
- 6 badge types with automatic awarding:
  - Contributor: 100 total points
  - Expert: 500 total points
  - Master: 1000 total points
  - Problem Solver: 10 accepted answers
  - Great Question: Question with 50 upvotes
  - Great Answer: Answer with 50 upvotes
- Automatic level and rank calculation
- Reputation statistics:
  - Questions asked
  - Answers provided
  - Accepted answers
  - Total points
  - Current level and rank

### Admin Features
- Dashboard with real-time statistics:
  - Total users
  - Total cars
  - Total bookings
  - Total revenue
  - Active bookings
  - Pending bookings
  - New users this month
  - New cars this month
- Business metrics with monthly trends:
  - Revenue by month
  - Top performing cars by revenue
  - Booking completion rates
- User management:
  - List all users with pagination
  - Search users by name or email
  - Filter by role
  - View user statistics
- Role management:
  - Assign roles to users
  - Remove roles from users
  - Available roles: Admin, User, Moderator

## Infrastructure Services

### Caching Service
- Redis caching with automatic fallback to memory cache
- ICacheService interface for abstraction
- Operations:
  - Get
  - Set with expiration
  - Remove
  - Exists
- Configurable expiration times
- Ready for use in controllers

### Background Jobs (Hangfire)
- Automatic booking status updates
- Expired refresh token cleanup
- Recurring job scheduling
- Job dashboard at /hangfire (Admin only)
- SQL Server storage for job persistence

### Email Service
- SMTP email sending with MailKit
- Configurable email settings
- Support for:
  - Plain text emails
  - HTML emails
  - Attachments
- Ready for notifications and verification emails

### Logging
- Serilog integration
- File logging with rolling files
- Console logging
- Structured logging
- Request/response logging middleware
- Performance monitoring middleware

### Health Checks
- Database health check
- Redis health check
- Health endpoints:
  - /health - Overall health
  - /health/ready - Readiness check
  - /health/live - Liveness check

## Security Features

### Security Headers
- HSTS (HTTP Strict Transport Security)
- X-Content-Type-Options: nosniff
- X-Frame-Options: DENY
- X-XSS-Protection: 1; mode=block
- Referrer-Policy: strict-origin-when-cross-origin
- Permissions-Policy (geolocation, microphone, camera disabled)
- Content Security Policy (CSP) with strict rules
- Server header removal

### CORS Configuration
- Development policy (AllowAll for testing)
- Production policy (restricted origins from configuration)
- Credentials support
- Wildcard subdomain support

### Rate Limiting
- IP-based rate limiting with AspNetCoreRateLimit
- Configurable limits per endpoint
- 429 status code for rate limit exceeded
- Memory-based counter storage

### Input Validation
- FluentValidation for all commands
- Validation behaviors in MediatR pipeline
- Comprehensive validation rules:
  - String length validation
  - Email format validation
  - Phone number format validation
  - Date range validation
  - Enum validation
  - Business rule validation

### SQL Injection Prevention
- All queries use Entity Framework Core with parameterization
- No raw SQL queries without parameters
- Repository pattern enforces safe data access
- No string concatenation in queries

## Performance Optimization

### Database Indexes
- Indexes on frequently queried columns:
  - Cars: City, State, Year, DailyRate, CarType, IsAvailable, OwnerId
  - Bookings: CarId, UserId, Status, StartDate, EndDate, BookingNumber
  - Reviews: CarId, UserId, Rating, IsVerified, CreatedAt
  - Questions: UserId, Category, CreatedAt, VoteCount, AnswerCount, ViewCount
  - Answers: QuestionId, UserId, VoteCount, IsAccepted, CreatedAt
  - UserReputation: UserId, TotalPoints, Level
  - RefreshToken: UserId, Token, ExpiresAt
- Composite indexes for common query patterns

### Query Optimization
- Projection with `.Select()` to avoid over-fetching
- Pagination support everywhere (PaginatedList<T>)
- Filtering at database level (Where clauses before materialization)
- Eager loading with `.Include()` where needed
- Soft delete query filters at DbContext level

### Caching Infrastructure
- Redis cache service ready for use
- Memory cache fallback
- Caching candidates identified:
  - Car listings (5 minutes)
  - User profiles (10 minutes)
  - Dashboard statistics (1 minute)
  - Leaderboards (5 minutes)
  - Review statistics (5 minutes)

## Technical Features

### Architecture Patterns
- Clean Architecture with clear layer separation
- CQRS pattern with MediatR
- Repository and Unit of Work patterns
- Result pattern for consistent error handling
- Specification pattern for complex queries

### Middleware
- Exception handling middleware (global error handling)
- Request logging middleware (request/response logging)
- Performance monitoring middleware (slow request detection)
- Security headers middleware

### API Documentation
- Swagger/OpenAPI with interactive UI
- XML documentation comments on all controllers
- Request/response examples
- Authentication support in Swagger UI
- Custom HTML documentation page at /Docs.html

### Soft Delete
- Soft delete implementation for all entities
- Query filters at DbContext level
- IsDeleted flag on entities
- Automatic filtering in queries

### Audit Fields
- CreatedAt timestamp on all entities
- UpdatedAt timestamp on all entities
- Automatic timestamp management

## Future Enhancements (Not Yet Implemented)

The following features are planned but not yet implemented:

### Community Posts
- Create and share posts
- Like and comment system
- Image attachments
- Categories and tags

### Community Events
- Create and manage events
- Event registration
- Attendance tracking
- Location-based events

### Community Groups
- Create and join groups
- Membership management
- Group invitations
- Private/public groups

### Additional Authentication Features
- Email verification
- Password reset via email
- Two-factor authentication
- Refresh token rotation

For implementation status, see [IMPLEMENTATION_PLAN.md](IMPLEMENTATION_PLAN.md).
