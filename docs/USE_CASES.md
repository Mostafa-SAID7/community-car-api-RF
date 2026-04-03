# Use Cases

This document provides practical examples of how to use the Community Car API endpoints.

## Authentication

### Register a New User
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "SecurePass123!",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "user": {
      "id": "user-guid",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "roles": ["User"]
    }
  }
}
```

### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "SecurePass123!"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "user": {
      "id": "user-guid",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "roles": ["User"]
    }
  }
}
```

## Car Management

### List Available Cars with Filters
```http
GET /api/cars?city=NewYork&carType=Sedan&minSeats=4&maxDailyRate=100&isAvailable=true
Authorization: Bearer {token}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": [
    {
      "id": "car-guid",
      "make": "Toyota",
      "model": "Camry",
      "year": 2023,
      "color": "Silver",
      "carType": "Sedan",
      "fuelType": "Gasoline",
      "transmission": "Automatic",
      "seats": 5,
      "description": "Well-maintained family car",
      "hourlyRate": 15.00,
      "dailyRate": 80.00,
      "city": "New York",
      "state": "NY",
      "imageUrl": null,
      "isAvailable": true
    }
  ]
}
```

### Get Car Details
```http
GET /api/cars/{carId}
Authorization: Bearer {token}
```

### Create a Car Listing
```http
POST /api/cars
Authorization: Bearer {token}
Content-Type: application/json

{
  "make": "Toyota",
  "model": "Camry",
  "year": 2023,
  "color": "Silver",
  "licensePlate": "ABC123",
  "carType": 0,
  "fuelType": 0,
  "transmission": 1,
  "seats": 5,
  "description": "Well-maintained family car",
  "hourlyRate": 15.00,
  "dailyRate": 80.00,
  "location": "123 Main St, New York, NY",
  "city": "New York",
  "state": "NY"
}
```

### Update a Car
```http
PUT /api/cars/{carId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "make": "Toyota",
  "model": "Camry",
  "year": 2023,
  "color": "Blue",
  "dailyRate": 75.00,
  "isAvailable": true
}
```

### Delete a Car
```http
DELETE /api/cars/{carId}
Authorization: Bearer {token}
```

## Booking System

### Create a Booking
```http
POST /api/bookings
Authorization: Bearer {token}
Content-Type: application/json

{
  "carId": "car-guid",
  "startDate": "2024-03-01T10:00:00Z",
  "endDate": "2024-03-05T10:00:00Z",
  "pickupLocation": "123 Main St, New York, NY",
  "dropoffLocation": "123 Main St, New York, NY"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "id": "booking-guid",
    "bookingNumber": "BK1234567890",
    "carId": "car-guid",
    "userId": "user-guid",
    "startDate": "2024-03-01T10:00:00Z",
    "endDate": "2024-03-05T10:00:00Z",
    "totalPrice": 320.00,
    "status": "Pending",
    "paymentStatus": "Pending"
  }
}
```

### Get My Bookings
```http
GET /api/bookings?status=Confirmed
Authorization: Bearer {token}
```

### Get Booking Details
```http
GET /api/bookings/{bookingId}
Authorization: Bearer {token}
```

### Cancel a Booking
```http
POST /api/bookings/{bookingId}/cancel
Authorization: Bearer {token}
```

## Review System

### Create a Review
```http
POST /api/reviews
Authorization: Bearer {token}
Content-Type: application/json

{
  "carId": "car-guid",
  "rating": 5,
  "comment": "Excellent car! Clean and drives smoothly. Highly recommend!"
}
```

### Get Reviews for a Car
```http
GET /api/reviews?carId={carId}
Authorization: Bearer {token}
```

### Get Review Statistics
```http
GET /api/reviews/statistics?carId={carId}
Authorization: Bearer {token}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "averageRating": 4.5,
    "totalReviews": 10,
    "fiveStarCount": 6,
    "fourStarCount": 3,
    "threeStarCount": 1,
    "twoStarCount": 0,
    "oneStarCount": 0
  }
}
```

### Update a Review
```http
PUT /api/reviews/{reviewId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "rating": 4,
  "comment": "Good car, minor issues with AC"
}
```

### Delete a Review
```http
DELETE /api/reviews/{reviewId}
Authorization: Bearer {token}
```

## User Profile Management

### Get My Profile
```http
GET /api/users/profile
Authorization: Bearer {token}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "id": "user-guid",
    "email": "john.doe@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "phoneNumber": "+1234567890",
    "roles": ["User"]
  }
}
```

### Update Profile
```http
PUT /api/users/profile
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890"
}
```

### Change Password
```http
POST /api/users/change-password
Authorization: Bearer {token}
Content-Type: application/json

{
  "currentPassword": "OldPass123!",
  "newPassword": "NewPass123!"
}
```

### Get User Statistics
```http
GET /api/users/statistics
Authorization: Bearer {token}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "totalBookings": 5,
    "carsOwned": 2,
    "totalSpent": 1500.00
  }
}
```

## Community Q&A

### Ask a Question
```http
POST /api/qa/questions
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Best practices for car maintenance?",
  "content": "What are the essential maintenance tasks for keeping a car in good condition?",
  "category": 1,
  "tags": "maintenance,tips,best-practices"
}
```

### Get Questions with Filters
```http
GET /api/qa/questions?category=1&search=maintenance&sortBy=Votes
Authorization: Bearer {token}
```

### Get Question Details
```http
GET /api/qa/questions/{questionId}
Authorization: Bearer {token}
```

### Answer a Question
```http
POST /api/qa/answers
Authorization: Bearer {token}
Content-Type: application/json

{
  "questionId": "question-guid",
  "content": "Regular oil changes every 5,000 miles, tire rotation every 6 months, and checking fluid levels monthly are essential."
}
```

### Vote on a Question
```http
POST /api/qa/questions/{questionId}/vote
Authorization: Bearer {token}
Content-Type: application/json

{
  "voteType": 0
}
```

Note: VoteType: 0 = Upvote, 1 = Downvote

### Vote on an Answer
```http
POST /api/qa/answers/{answerId}/vote
Authorization: Bearer {token}
Content-Type: application/json

{
  "voteType": 0
}
```

### Accept an Answer
```http
POST /api/qa/answers/{answerId}/accept
Authorization: Bearer {token}
```

### Get Trending Questions
```http
GET /api/qa/questions/trending
```

### Get My Questions
```http
GET /api/qa/questions/my
Authorization: Bearer {token}
```

### Get Leaderboard
```http
GET /api/qa/leaderboard?period=Monthly&pageNumber=1&pageSize=10
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "items": [
      {
        "userId": "user-guid",
        "userName": "John Doe",
        "totalPoints": 250,
        "level": 2,
        "rank": "Contributor",
        "questionsAsked": 10,
        "answersProvided": 15,
        "acceptedAnswers": 5
      }
    ],
    "pageNumber": 1,
    "totalPages": 1,
    "totalCount": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  }
}
```

### Get User Reputation
```http
GET /api/qa/reputation
Authorization: Bearer {token}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "userId": "user-guid",
    "totalPoints": 250,
    "level": 2,
    "rank": "Contributor",
    "questionsAsked": 10,
    "answersProvided": 15,
    "acceptedAnswers": 5,
    "badges": [
      {
        "name": "Contributor",
        "description": "Earned 100 total points",
        "badgeType": "Contributor",
        "iconUrl": null,
        "earnedAt": "2024-01-15T10:00:00Z"
      }
    ]
  }
}
```

### Delete a Question
```http
DELETE /api/qa/questions/{questionId}
Authorization: Bearer {token}
```

### Delete an Answer
```http
DELETE /api/qa/answers/{answerId}
Authorization: Bearer {token}
```

## Admin Operations

### Get Dashboard Statistics
```http
GET /api/admin/dashboard/statistics
Authorization: Bearer {admin-token}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "totalUsers": 150,
    "totalCars": 75,
    "totalBookings": 300,
    "totalRevenue": 25000.00,
    "activeBookings": 15,
    "pendingBookings": 5,
    "newUsersThisMonth": 20,
    "newCarsThisMonth": 10
  }
}
```

### Get Business Metrics
```http
GET /api/admin/dashboard/metrics
Authorization: Bearer {admin-token}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "monthlyRevenue": [
      { "month": "January", "revenue": 5000.00 },
      { "month": "February", "revenue": 6000.00 }
    ],
    "topCars": [
      {
        "carId": "car-guid",
        "make": "Toyota",
        "model": "Camry",
        "totalRevenue": 2000.00,
        "bookingCount": 10
      }
    ],
    "completionRate": 95.5
  }
}
```

### Get All Users
```http
GET /api/admin/users?search=john&role=User&pageNumber=1&pageSize=20
Authorization: Bearer {admin-token}
```

### Assign Role to User
```http
POST /api/admin/users/{userId}/roles
Authorization: Bearer {admin-token}
Content-Type: application/json

{
  "role": "Moderator"
}
```

### Remove Role from User
```http
DELETE /api/admin/users/{userId}/roles/Moderator
Authorization: Bearer {admin-token}
```

## Common Workflows

### Workflow 1: Complete Booking Process
1. User searches for available cars with filters
2. User views car details and reviews
3. User creates a booking
4. Booking is confirmed (status: Pending → Confirmed)
5. After trip, booking status changes to Completed
6. User leaves a review for the car

### Workflow 2: Community Engagement
1. User asks a question in Q&A
2. Other users provide answers
3. Users vote on questions and answers
4. Question asker accepts the best answer
5. Users earn points and badges
6. Users level up based on points

### Workflow 3: Car Owner Journey
1. Owner registers account
2. Owner creates car listing
3. Car appears in search results
4. Users book the car
5. Owner receives bookings
6. Owner gets reviews from renters

### Workflow 4: Admin Management
1. Admin monitors dashboard statistics
2. Admin reviews business metrics
3. Admin manages users (assign/remove roles)
4. Admin tracks system performance

## Error Handling Examples

### Validation Error
```json
{
  "isSuccess": false,
  "error": {
    "code": "ValidationError",
    "message": "One or more validation errors occurred",
    "details": {
      "Email": ["Email is required"],
      "Password": ["Password must be at least 8 characters"]
    }
  }
}
```

### Not Found Error
```json
{
  "isSuccess": false,
  "error": {
    "code": "NotFound",
    "message": "Car not found"
  }
}
```

### Unauthorized Error
```json
{
  "isSuccess": false,
  "error": {
    "code": "Unauthorized",
    "message": "You are not authorized to perform this action"
  }
}
```

### Business Logic Error
```json
{
  "isSuccess": false,
  "error": {
    "code": "BookingConflict",
    "message": "Car is not available for the selected dates"
  }
}
```

## Rate Limiting

All endpoints are rate-limited to prevent abuse:
- Default: 60 requests per minute per IP
- Configurable in appsettings.json

Rate limit headers in response:
```
X-Rate-Limit-Limit: 60
X-Rate-Limit-Remaining: 59
X-Rate-Limit-Reset: 1234567890
```

When rate limit is exceeded:
```json
{
  "statusCode": 429,
  "message": "Too many requests. Please try again later."
}
```

## Enum Values Reference

### CarType
- 0: Sedan
- 1: SUV
- 2: Truck
- 3: Van
- 4: Coupe
- 5: Convertible
- 6: Hatchback
- 7: Wagon

### FuelType
- 0: Gasoline
- 1: Diesel
- 2: Electric
- 3: Hybrid

### TransmissionType
- 0: Manual
- 1: Automatic

### BookingStatus
- 0: Pending
- 1: Confirmed
- 2: Completed
- 3: Cancelled

### QuestionCategory
- 0: General
- 1: Maintenance
- 2: Insurance
- 3: Safety
- 4: FuelEfficiency
- 5: BuyingGuide
- 6: SellingTips
- 7: Legal
- 8: Technology
- 9: Modifications

### VoteType
- 0: Upvote
- 1: Downvote

For complete API documentation, see [API_REFERENCE.md](API_REFERENCE.md).
