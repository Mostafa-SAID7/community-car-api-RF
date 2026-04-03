# Entity Relationship Diagram (ERD)

## Database Schema Overview

The Community Car API uses a relational database design following Clean Architecture principles with Entity Framework Core.

## Core Entities

### User Management
```
ApplicationUser (ASP.NET Core Identity)
├── Id (Guid, PK)
├── UserName (string)
├── Email (string)
├── PhoneNumber (string)
├── FirstName (string)
├── LastName (string)
├── ProfileImageUrl (string)
├── Bio (string)
├── DateOfBirth (DateTime)
├── Address (string)
├── City (string)
├── State (string)
├── ZipCode (string)
├── Country (string)
├── IsEmailVerified (bool)
├── IsPhoneVerified (bool)
├── TwoFactorEnabled (bool)
├── LockoutEnabled (bool)
├── AccessFailedCount (int)
├── CreatedAt (DateTime)
├── UpdatedAt (DateTime)
└── Relationships:
    ├── Cars (1:N)
    ├── Bookings (1:N)
    ├── Reviews (1:N)
    ├── Posts (1:N)
    ├── Questions (1:N)
    ├── Answers (1:N)
    └── RefreshTokens (1:N)
```

### Car Management
```
Car
├── Id (Guid, PK)
├── OwnerId (Guid, FK -> ApplicationUser)
├── Make (string)
├── Model (string)
├── Year (int)
├── Color (string)
├── LicensePlate (string)
├── VIN (string)
├── CarType (enum: Sedan, SUV, Truck, etc.)
├── FuelType (enum: Gasoline, Diesel, Electric, Hybrid)
├── Transmission (enum: Manual, Automatic)
├── Seats (int)
├── Description (string)
├── HourlyRate (decimal)
├── DailyRate (decimal)
├── WeeklyRate (decimal)
├── MonthlyRate (decimal)
├── Mileage (int)
├── Location (string)
├── City (string)
├── State (string)
├── Latitude (double)
├── Longitude (double)
├── IsAvailable (bool)
├── IsVerified (bool)
├── PrimaryImageUrl (string)
├── Features (string, JSON)
├── Rules (string)
├── CreatedAt (DateTime)
├── UpdatedAt (DateTime)
├── IsDeleted (bool)
└── Relationships:
    ├── Owner (N:1 -> ApplicationUser)
    ├── Images (1:N -> CarImage)
    ├── Bookings (1:N)
    └── Reviews (1:N)
```

### Booking System
```
Booking
├── Id (Guid, PK)
├── BookingNumber (string, unique)
├── CarId (Guid, FK -> Car)
├── UserId (Guid, FK -> ApplicationUser)
├── StartDate (DateTime)
├── EndDate (DateTime)
├── PickupLocation (string)
├── DropoffLocation (string)
├── TotalAmount (decimal)
├── Status (enum: Pending, Confirmed, InProgress, Completed, Cancelled)
├── PaymentStatus (enum: Pending, Paid, Refunded, Failed)
├── PaymentMethod (string)
├── PaymentTransactionId (string)
├── Notes (string)
├── CancellationReason (string)
├── CancelledAt (DateTime?)
├── CreatedAt (DateTime)
├── UpdatedAt (DateTime)
├── IsDeleted (bool)
└── Relationships:
    ├── Car (N:1)
    ├── User (N:1)
    └── Review (1:1)
```

### Review System
```
Review
├── Id (Guid, PK)
├── UserId (Guid, FK -> ApplicationUser)
├── EntityId (Guid)
├── EntityType (enum: Car, User, Event, etc.)
├── Rating (int, 1-5)
├── Title (string)
├── Comment (string)
├── Images (string, JSON array)
├── IsVerified (bool)
├── HelpfulCount (int)
├── NotHelpfulCount (int)
├── CreatedAt (DateTime)
├── UpdatedAt (DateTime)
├── IsDeleted (bool)
└── Relationships:
    ├── User (N:1)
    └── Votes (1:N -> ReviewVote)
```

### Community Q&A
```
Question
├── Id (Guid, PK)
├── UserId (Guid, FK -> ApplicationUser)
├── Title (string)
├── Content (string)
├── Category (enum: Maintenance, BuyingGuide, Troubleshooting, etc.)
├── Tags (string, comma-separated)
├── ViewCount (int)
├── VoteCount (int)
├── AnswerCount (int)
├── IsSolved (bool)
├── IsVerified (bool)
├── Points (int)
├── CreatedAt (DateTime)
├── UpdatedAt (DateTime)
├── IsDeleted (bool)
└── Relationships:
    ├── User (N:1)
    ├── Answers (1:N)
    └── Votes (1:N -> QuestionVote)

Answer
├── Id (Guid, PK)
├── QuestionId (Guid, FK -> Question)
├── UserId (Guid, FK -> ApplicationUser)
├── Content (string)
├── VoteCount (int)
├── IsAccepted (bool)
├── IsVerified (bool)
├── Points (int)
├── CreatedAt (DateTime)
├── UpdatedAt (DateTime)
├── IsDeleted (bool)
└── Relationships:
    ├── Question (N:1)
    ├── User (N:1)
    └── Votes (1:N -> AnswerVote)
```

### Community Posts
```
Post
├── Id (Guid, PK)
├── UserId (Guid, FK -> ApplicationUser)
├── Title (string)
├── Content (string)
├── ImageUrl (string)
├── Category (string)
├── Tags (string)
├── LikeCount (int)
├── CommentCount (int)
├── ViewCount (int)
├── IsPublished (bool)
├── CreatedAt (DateTime)
├── UpdatedAt (DateTime)
├── IsDeleted (bool)
└── Relationships:
    ├── User (N:1)
    ├── Comments (1:N)
    └── Likes (1:N -> PostLike)
```

### Community Events
```
Event
├── Id (Guid, PK)
├── OrganizerId (Guid, FK -> ApplicationUser)
├── Title (string)
├── Description (string)
├── Location (string)
├── Address (string)
├── City (string)
├── State (string)
├── StartDate (DateTime)
├── EndDate (DateTime)
├── MaxAttendees (int)
├── CurrentAttendees (int)
├── ImageUrl (string)
├── Category (string)
├── IsFree (bool)
├── Price (decimal)
├── Status (enum: Draft, Published, Cancelled, Completed)
├── CreatedAt (DateTime)
├── UpdatedAt (DateTime)
├── IsDeleted (bool)
└── Relationships:
    ├── Organizer (N:1 -> ApplicationUser)
    └── Registrations (1:N -> EventRegistration)
```

### Community Groups
```
Group
├── Id (Guid, PK)
├── CreatorId (Guid, FK -> ApplicationUser)
├── Name (string)
├── Description (string)
├── ImageUrl (string)
├── Category (string)
├── IsPrivate (bool)
├── MemberCount (int)
├── MaxMembers (int)
├── CreatedAt (DateTime)
├── UpdatedAt (DateTime)
├── IsDeleted (bool)
└── Relationships:
    ├── Creator (N:1 -> ApplicationUser)
    └── Members (1:N -> GroupMember)
```

### Gamification
```
UserBadge
├── Id (Guid, PK)
├── UserId (Guid, FK -> ApplicationUser)
├── BadgeType (enum)
├── Name (string)
├── Description (string)
├── IconUrl (string)
├── Points (int)
├── EarnedAt (DateTime)
└── Relationships:
    └── User (N:1)

UserPoints
├── Id (Guid, PK)
├── UserId (Guid, FK -> ApplicationUser)
├── TotalPoints (int)
├── Level (int)
├── Rank (string)
└── Relationships:
    └── User (N:1)
```

### Admin & Security
```
AuditLog
├── Id (Guid, PK)
├── UserId (Guid?)
├── Action (string)
├── EntityType (string)
├── EntityId (Guid?)
├── OldValues (string, JSON)
├── NewValues (string, JSON)
├── IpAddress (string)
├── UserAgent (string)
├── Timestamp (DateTime)

SecurityEvent
├── Id (Guid, PK)
├── EventType (string)
├── Severity (enum: Low, Medium, High, Critical)
├── Description (string)
├── IpAddress (string)
├── UserId (Guid?)
├── IsResolved (bool)
├── CreatedAt (DateTime)
```

## Relationships Summary

### One-to-Many (1:N)
- User → Cars
- User → Bookings
- User → Reviews
- User → Questions
- User → Answers
- User → Posts
- User → Events
- User → Groups
- Car → Bookings
- Car → Reviews
- Question → Answers
- Post → Comments
- Event → Registrations
- Group → Members

### Many-to-Many (N:M)
- Users ↔ Groups (through GroupMember)
- Users ↔ Events (through EventRegistration)
- Posts ↔ Users (through PostLike)

### One-to-One (1:1)
- Booking → Review (optional)

## Indexes

### Performance Indexes
- Car: (City, State, IsAvailable, IsDeleted)
- Booking: (UserId, Status, IsDeleted)
- Booking: (CarId, StartDate, EndDate)
- Review: (EntityId, EntityType, IsDeleted)
- Question: (Category, IsDeleted, CreatedAt)
- Post: (UserId, IsPublished, IsDeleted)
- Event: (StartDate, Status, IsDeleted)

### Unique Indexes
- Car: (LicensePlate)
- Booking: (BookingNumber)
- ApplicationUser: (Email)
- ApplicationUser: (UserName)

## Database Diagram

```
┌─────────────────┐
│ ApplicationUser │
└────────┬────────┘
         │
         ├──────────┬──────────┬──────────┬──────────┐
         │          │          │          │          │
    ┌────▼───┐ ┌───▼────┐ ┌───▼─────┐ ┌──▼──────┐ ┌▼────────┐
    │  Car   │ │Booking │ │ Review  │ │Question │ │  Post   │
    └────┬───┘ └───┬────┘ └─────────┘ └────┬────┘ └─────────┘
         │         │                        │
         │         │                   ┌────▼────┐
         │         │                   │ Answer  │
         │         │                   └─────────┘
         │         │
         └─────────┘
```

## Notes

- All entities inherit from `BaseEntity` which includes:
  - Id (Guid)
  - CreatedAt (DateTime)
  - UpdatedAt (DateTime)
  - IsDeleted (bool) for soft delete

- Audit fields are automatically managed by EF Core interceptors

- All foreign keys use Guid type for better distribution and security

- Soft delete is implemented across all entities for data integrity
