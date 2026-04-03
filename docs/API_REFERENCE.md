# Community Car API Reference

## Base URL
```
http://localhost:5075/api
```

## Authentication
Most endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

---

## Authentication Endpoints

### Register
Create a new user account.

**Endpoint:** `POST /api/auth/register`

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890"
}
```

**Response:** `200 OK`
```json
{
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "email": "user@example.com",
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "refresh_token_here"
}
```

### Login
Authenticate and receive JWT token.

**Endpoint:** `POST /api/auth/login`

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

**Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "refresh_token_here",
  "expiresIn": 3600
}
```

---

## Car Management

### Get All Cars
Retrieve all available cars with optional filters.

**Endpoint:** `GET /api/cars`

**Query Parameters:**
- `city` (string, optional): Filter by city
- `state` (string, optional): Filter by state
- `minYear` (int, optional): Minimum year
- `maxYear` (int, optional): Maximum year
- `carType` (int, optional): Car type enum
- `minPrice` (decimal, optional): Minimum daily rate
- `maxPrice` (decimal, optional): Maximum daily rate

**Response:** `200 OK`
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ownerId": "550e8400-e29b-41d4-a716-446655440000",
    "make": "Toyota",
    "model": "Camry",
    "year": 2023,
    "color": "Silver",
    "carType": "Sedan",
    "fuelType": "Gasoline",
    "transmission": "Automatic",
    "seats": 5,
    "hourlyRate": 15.00,
    "dailyRate": 80.00,
    "city": "New York",
    "state": "NY",
    "isAvailable": true
  }
]
```

### Get Car by ID
Retrieve a specific car by its ID.

**Endpoint:** `GET /api/cars/{id}`

**Response:** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "make": "Toyota",
  "model": "Camry",
  "year": 2023,
  "description": "Well-maintained family car",
  "hourlyRate": 15.00,
  "dailyRate": 80.00,
  "isAvailable": true
}
```

### Create Car
Add a new car listing (requires authentication).

**Endpoint:** `POST /api/cars`

**Request Body:**
```json
{
  "make": "Toyota",
  "model": "Camry",
  "year": 2023,
  "color": "Silver",
  "licensePlate": "ABC123",
  "carType": 1,
  "fuelType": 1,
  "transmission": 1,
  "seats": 5,
  "description": "Well-maintained family car",
  "hourlyRate": 15.00,
  "dailyRate": 80.00,
  "mileage": 25000,
  "city": "New York",
  "state": "NY"
}
```

**Response:** `201 Created`

### Update Car
Update an existing car (requires authentication and ownership).

**Endpoint:** `PUT /api/cars/{id}`

**Request Body:** Same as Create Car

**Response:** `200 OK`

### Delete Car
Soft delete a car (requires authentication and ownership).

**Endpoint:** `DELETE /api/cars/{id}`

**Response:** `204 No Content`

---

## Booking Management

### Get All Bookings
Retrieve bookings with optional filters.

**Endpoint:** `GET /api/bookings`

**Query Parameters:**
- `carId` (guid, optional): Filter by car
- `userId` (string, optional): Filter by user
- `status` (int, optional): Filter by status
- `startDate` (datetime, optional): Filter by start date
- `endDate` (datetime, optional): Filter by end date

**Response:** `200 OK`
```json
[
  {
    "id": "7fa85f64-5717-4562-b3fc-2c963f66afa6",
    "bookingNumber": "BK20240302153045",
    "carId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "startDate": "2024-03-01T10:00:00Z",
    "endDate": "2024-03-05T10:00:00Z",
    "totalAmount": 320.00,
    "status": "Confirmed",
    "paymentStatus": "Paid"
  }
]
```

### Create Booking
Create a new booking (requires authentication).

**Endpoint:** `POST /api/bookings`

**Request Body:**
```json
{
  "carId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "startDate": "2024-03-01T10:00:00Z",
  "endDate": "2024-03-05T10:00:00Z",
  "notes": "Need GPS navigation"
}
```

**Response:** `201 Created`

### Cancel Booking
Cancel an existing booking (requires authentication and ownership).

**Endpoint:** `POST /api/bookings/{id}/cancel`

**Response:** `200 OK`

---

## Review System

### Get Reviews
Retrieve reviews with optional filters.

**Endpoint:** `GET /api/reviews`

**Query Parameters:**
- `carId` (guid, optional): Filter by car
- `reviewerId` (string, optional): Filter by reviewer
- `minRating` (int, optional): Minimum rating (1-5)
- `isVerified` (bool, optional): Filter verified reviews

**Response:** `200 OK`
```json
[
  {
    "id": "9fa85f64-5717-4562-b3fc-2c963f66afa6",
    "reviewerId": "550e8400-e29b-41d4-a716-446655440000",
    "reviewerName": "John Doe",
    "carId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "rating": 5,
    "comment": "Excellent car, very clean!",
    "isVerified": true,
    "createdAt": "2024-03-02T15:30:45Z"
  }
]
```

### Get Review Statistics
Get aggregated review statistics for a car or user.

**Endpoint:** `GET /api/reviews/statistics`

**Query Parameters:**
- `carId` (guid, optional): Car ID
- `reviewedUserId` (string, optional): User ID

**Response:** `200 OK`
```json
{
  "totalReviews": 25,
  "averageRating": 4.6,
  "fiveStars": 15,
  "fourStars": 8,
  "threeStars": 2,
  "twoStars": 0,
  "oneStar": 0
}
```

### Create Review
Create a new review (requires authentication and completed booking).

**Endpoint:** `POST /api/reviews`

**Request Body:**
```json
{
  "carId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "rating": 5,
  "comment": "Excellent car, very clean!"
}
```

**Response:** `201 Created`

---

## User Management

### Get Profile
Get current user's profile (requires authentication).

**Endpoint:** `GET /api/users/profile`

**Response:** `200 OK`
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "city": "New York",
  "state": "NY",
  "roles": ["User"]
}
```

### Update Profile
Update user profile (requires authentication).

**Endpoint:** `PUT /api/users/profile`

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "city": "New York",
  "state": "NY"
}
```

**Response:** `200 OK`

### Change Password
Change user password (requires authentication).

**Endpoint:** `POST /api/users/change-password`

**Request Body:**
```json
{
  "currentPassword": "OldPass123!",
  "newPassword": "NewPass123!"
}
```

**Response:** `200 OK`

### Get User Statistics
Get user's booking and car statistics (requires authentication).

**Endpoint:** `GET /api/users/statistics`

**Response:** `200 OK`
```json
{
  "totalBookings": 15,
  "completedBookings": 12,
  "totalCarsOwned": 2,
  "totalSpent": 1250.00,
  "totalEarned": 3500.00
}
```

---

## Admin Endpoints

All admin endpoints require the `Admin` role.

### Get Dashboard Statistics
Get overall platform statistics.

**Endpoint:** `GET /api/admin/dashboard/statistics`

**Response:** `200 OK`
```json
{
  "totalUsers": 1250,
  "totalCars": 450,
  "totalBookings": 3200,
  "totalReviews": 1800,
  "activeBookings": 45,
  "pendingBookings": 12,
  "totalRevenue": 125000.00,
  "monthlyRevenue": 15000.00,
  "newUsersThisMonth": 85,
  "newCarsThisMonth": 23,
  "averageRating": 4.6
}
```

### Get Business Metrics
Get detailed business metrics and trends.

**Endpoint:** `GET /api/admin/dashboard/metrics`

**Query Parameters:**
- `monthsBack` (int, optional, default: 6): Number of months to analyze

**Response:** `200 OK`
```json
{
  "totalRevenue": 125000.00,
  "averageBookingValue": 95.50,
  "totalBookings": 3200,
  "completedBookings": 2850,
  "cancelledBookings": 350,
  "bookingCompletionRate": 89.06,
  "monthlyRevenues": [
    {
      "month": "2024-01",
      "revenue": 18500.00,
      "bookingCount": 245
    }
  ],
  "topCars": [
    {
      "carId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "make": "Toyota",
      "model": "Camry",
      "bookingCount": 85,
      "totalRevenue": 6800.00
    }
  ]
}
```

### Get Users (Admin)
Get all users with filters and pagination.

**Endpoint:** `GET /api/admin/users`

**Query Parameters:**
- `searchTerm` (string, optional): Search by email or name
- `role` (string, optional): Filter by role
- `isEmailVerified` (bool, optional): Filter by email verification
- `pageNumber` (int, default: 1): Page number
- `pageSize` (int, default: 20): Items per page

**Response:** `200 OK`

### Assign Role
Assign a role to a user.

**Endpoint:** `POST /api/admin/users/{userId}/roles`

**Request Body:**
```json
{
  "role": "Admin"
}
```

**Response:** `200 OK`

### Remove Role
Remove a role from a user.

**Endpoint:** `DELETE /api/admin/users/{userId}/roles/{role}`

**Response:** `204 No Content`

---

## Error Responses

All endpoints may return the following error responses:

### 400 Bad Request
```json
{
  "error": "Validation failed",
  "errorCode": "VALIDATION_ERROR",
  "validationErrors": {
    "Email": ["Email is required"],
    "Password": ["Password must be at least 8 characters"]
  }
}
```

### 401 Unauthorized
```json
{
  "error": "User not authenticated",
  "errorCode": "UNAUTHORIZED"
}
```

### 403 Forbidden
```json
{
  "error": "You do not have permission to access this resource",
  "errorCode": "FORBIDDEN"
}
```

### 404 Not Found
```json
{
  "error": "Resource not found",
  "errorCode": "NOT_FOUND"
}
```

### 500 Internal Server Error
```json
{
  "error": "An error occurred while processing your request",
  "message": "Detailed error message",
  "statusCode": 500
}
```

---

## Rate Limiting

API requests are rate-limited to prevent abuse:
- **Anonymous users**: 100 requests per hour
- **Authenticated users**: 1000 requests per hour
- **Admin users**: Unlimited

---

## Pagination

List endpoints support pagination with the following query parameters:
- `pageNumber` (int, default: 1): Page number
- `pageSize` (int, default: 20, max: 100): Items per page

Response includes pagination metadata in headers:
```
X-Pagination: {"CurrentPage":1,"TotalPages":5,"PageSize":20,"TotalCount":95}
```

---

## Enums

### CarType
- 0: Sedan
- 1: SUV
- 2: Truck
- 3: Van
- 4: Coupe
- 5: Convertible

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
- 2: Cancelled
- 3: Completed

### PaymentStatus
- 0: Pending
- 1: Paid
- 2: Failed
- 3: Refunded

---

## Webhooks (Coming Soon)

Webhook support for real-time notifications:
- Booking created
- Booking confirmed
- Booking cancelled
- Payment completed
- Review submitted

---

## Community Q&A System

The Community Q&A System enables users to ask questions, provide answers, vote on content, and build reputation through gamified participation.

### Ask a Question
Create a new question in the community.

**Endpoint:** `POST /api/qa/questions`

**Authentication:** Required

**Request Body:**
```json
{
  "title": "How do I maintain my shared car?",
  "content": "I'm new to car sharing and want to know the best practices for maintaining a shared vehicle. What should I check regularly?",
  "category": 1,
  "tags": "maintenance,best-practices,beginner"
}
```

**Request Fields:**
- `title` (string, required): Question title (10-200 characters)
- `content` (string, required): Question content (20-5000 characters)
- `category` (int, required): Question category (see Category enum below)
- `tags` (string, optional): Comma-separated tags (each tag 2-30 characters)

**Response:** `201 Created`
```json
{
  "isSuccess": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "userName": "John Doe",
    "title": "How do I maintain my shared car?",
    "content": "I'm new to car sharing and want to know the best practices...",
    "category": "Maintenance",
    "tags": ["maintenance", "best-practices", "beginner"],
    "voteCount": 0,
    "answerCount": 0,
    "viewCount": 0,
    "isSolved": false,
    "createdAt": "2024-03-02T15:30:45Z"
  }
}
```

**Points Awarded:** +5 points to the question author

---

### Get Questions with Filters
Retrieve questions with optional filtering, sorting, and pagination.

**Endpoint:** `GET /api/qa/questions`

**Authentication:** None (Public)

**Query Parameters:**
- `category` (int, optional): Filter by category (0-9)
- `searchTerm` (string, optional): Search in title or content
- `tag` (string, optional): Filter by specific tag
- `isSolved` (bool, optional): Filter by solved status
- `sortBy` (string, optional, default: "CreatedAt"): Sort field (CreatedAt, VoteCount, AnswerCount, ViewCount)
- `sortDescending` (bool, optional, default: true): Sort direction
- `pageNumber` (int, optional, default: 1): Page number
- `pageSize` (int, optional, default: 20): Items per page

**Example Request:**
```
GET /api/qa/questions?category=1&isSolved=false&sortBy=VoteCount&pageNumber=1&pageSize=10
```

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "userId": "550e8400-e29b-41d4-a716-446655440000",
        "userName": "John Doe",
        "title": "How do I maintain my shared car?",
        "content": "I'm new to car sharing...",
        "category": "Maintenance",
        "tags": ["maintenance", "best-practices"],
        "voteCount": 15,
        "answerCount": 3,
        "viewCount": 125,
        "isSolved": false,
        "createdAt": "2024-03-02T15:30:45Z"
      }
    ],
    "pageNumber": 1,
    "totalPages": 5,
    "totalCount": 48,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

---

### Get Question Details
Retrieve detailed information about a specific question including all answers.

**Endpoint:** `GET /api/qa/questions/{id}`

**Authentication:** None (Public)

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "userName": "John Doe",
    "title": "How do I maintain my shared car?",
    "content": "I'm new to car sharing and want to know the best practices...",
    "category": "Maintenance",
    "tags": ["maintenance", "best-practices", "beginner"],
    "voteCount": 15,
    "answerCount": 2,
    "viewCount": 126,
    "isSolved": true,
    "createdAt": "2024-03-02T15:30:45Z",
    "answers": [
      {
        "id": "7fa85f64-5717-4562-b3fc-2c963f66afa6",
        "questionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "userId": "650e8400-e29b-41d4-a716-446655440001",
        "userName": "Jane Smith",
        "content": "Regular maintenance is crucial. Check oil levels weekly...",
        "voteCount": 12,
        "isAccepted": true,
        "createdAt": "2024-03-02T16:45:30Z",
        "hasUserVoted": true,
        "userVoteType": 1
      }
    ],
    "hasUserVoted": false,
    "userVoteType": null
  }
}
```

**Note:** Viewing a question increments its `viewCount` by 1.

---

### Delete Question
Soft delete a question (author or admin only).

**Endpoint:** `DELETE /api/qa/questions/{id}`

**Authentication:** Required (Author or Admin role)

**Response:** `204 No Content`

**Error Codes:**
- `QA_QUESTION_NOT_FOUND`: Question does not exist
- `QA_UNAUTHORIZED_DELETE`: User is not the author or admin

**Note:** Deleting a question also soft deletes all associated answers.

---

### Get Trending Questions
Retrieve questions with high recent activity.

**Endpoint:** `GET /api/qa/questions/trending`

**Authentication:** None (Public)

**Query Parameters:**
- `limit` (int, optional, default: 20, max: 20): Number of results

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "userId": "550e8400-e29b-41d4-a716-446655440000",
      "userName": "John Doe",
      "title": "Best electric cars for sharing?",
      "content": "Looking for recommendations...",
      "category": "BuyingGuide",
      "tags": ["electric", "recommendations"],
      "voteCount": 45,
      "answerCount": 12,
      "viewCount": 523,
      "isSolved": false,
      "createdAt": "2024-03-01T10:00:00Z"
    }
  ]
}
```

**Note:** Trending score is calculated based on views, votes, and answers from the last 7 days.

---

### Get My Questions
Retrieve the current user's questions.

**Endpoint:** `GET /api/qa/questions/my`

**Authentication:** Required

**Query Parameters:**
- `pageNumber` (int, optional, default: 1): Page number
- `pageSize` (int, optional, default: 20): Items per page

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "userId": "550e8400-e29b-41d4-a716-446655440000",
        "userName": "John Doe",
        "title": "How do I maintain my shared car?",
        "content": "I'm new to car sharing...",
        "category": "Maintenance",
        "tags": ["maintenance"],
        "voteCount": 15,
        "answerCount": 3,
        "viewCount": 125,
        "isSolved": true,
        "createdAt": "2024-03-02T15:30:45Z"
      }
    ],
    "pageNumber": 1,
    "totalPages": 2,
    "totalCount": 8,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

---

### Answer a Question
Provide an answer to a question.

**Endpoint:** `POST /api/qa/answers`

**Authentication:** Required

**Request Body:**
```json
{
  "questionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "content": "Regular maintenance is crucial for shared cars. I recommend checking oil levels weekly, tire pressure monthly, and scheduling professional service every 6 months. Keep detailed maintenance logs to share with other users."
}
```

**Request Fields:**
- `questionId` (guid, required): ID of the question to answer
- `content` (string, required): Answer content (20-5000 characters)

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "id": "7fa85f64-5717-4562-b3fc-2c963f66afa6",
    "questionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "650e8400-e29b-41d4-a716-446655440001",
    "userName": "Jane Smith",
    "content": "Regular maintenance is crucial for shared cars...",
    "voteCount": 0,
    "isAccepted": false,
    "createdAt": "2024-03-02T16:45:30Z",
    "hasUserVoted": false,
    "userVoteType": null
  }
}
```

**Points Awarded:** +10 points to the answer author

**Error Codes:**
- `QA_QUESTION_NOT_FOUND`: Question does not exist
- `QA_DUPLICATE_ANSWER`: User has already answered this question

---

### Delete Answer
Soft delete an answer (author or admin only).

**Endpoint:** `DELETE /api/qa/answers/{id}`

**Authentication:** Required (Author or Admin role)

**Response:** `204 No Content`

**Error Codes:**
- `QA_ANSWER_NOT_FOUND`: Answer does not exist
- `QA_UNAUTHORIZED_DELETE`: User is not the author or admin

**Note:** If the deleted answer was accepted, the question's `isSolved` status is set to false.

---

### Vote on Question
Upvote or downvote a question.

**Endpoint:** `POST /api/qa/questions/{id}/vote`

**Authentication:** Required

**Request Body:**
```json
{
  "voteType": 1
}
```

**Request Fields:**
- `voteType` (int, required): 1 for upvote, -1 for downvote

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "newVoteCount": 16,
    "pointsAwarded": 5
  }
}
```

**Points Awarded:**
- Upvote: +5 points to question author
- Downvote: -2 points to question author

**Vote Behavior:**
- First vote: Records the vote
- Same vote again: Removes the vote (vote count adjusted accordingly)
- Different vote: Changes the vote (vote count changes by ±2)

**Error Codes:**
- `QA_QUESTION_NOT_FOUND`: Question does not exist
- `QA_CANNOT_VOTE_OWN`: Cannot vote on your own question

---

### Vote on Answer
Upvote or downvote an answer.

**Endpoint:** `POST /api/qa/answers/{id}/vote`

**Authentication:** Required

**Request Body:**
```json
{
  "voteType": 1
}
```

**Request Fields:**
- `voteType` (int, required): 1 for upvote, -1 for downvote

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "newVoteCount": 13,
    "pointsAwarded": 10
  }
}
```

**Points Awarded:**
- Upvote: +10 points to answer author
- Downvote: -5 points to answer author

**Vote Behavior:**
- First vote: Records the vote
- Same vote again: Removes the vote (vote count adjusted accordingly)
- Different vote: Changes the vote (vote count changes by ±2)

**Error Codes:**
- `QA_ANSWER_NOT_FOUND`: Answer does not exist
- `QA_CANNOT_VOTE_OWN`: Cannot vote on your own answer

---

### Accept Answer
Mark an answer as the accepted solution (question author only).

**Endpoint:** `POST /api/qa/answers/{id}/accept`

**Authentication:** Required (Question author only)

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "id": "7fa85f64-5717-4562-b3fc-2c963f66afa6",
    "questionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userId": "650e8400-e29b-41d4-a716-446655440001",
    "userName": "Jane Smith",
    "content": "Regular maintenance is crucial for shared cars...",
    "voteCount": 13,
    "isAccepted": true,
    "createdAt": "2024-03-02T16:45:30Z",
    "hasUserVoted": true,
    "userVoteType": 1
  }
}
```

**Points Awarded:** +25 points to the answer author

**Behavior:**
- Sets the answer's `isAccepted` to true
- Sets the question's `isSolved` to true
- If another answer was previously accepted, it is unaccepted

**Error Codes:**
- `QA_ANSWER_NOT_FOUND`: Answer does not exist
- `QA_UNAUTHORIZED_ACCEPT`: Only the question author can accept answers

---

### Get Leaderboard
Retrieve the community leaderboard ranked by reputation points.

**Endpoint:** `GET /api/qa/leaderboard`

**Authentication:** None (Public)

**Query Parameters:**
- `timePeriod` (string, optional, default: "AllTime"): Time period filter (AllTime, Monthly, Weekly)
- `pageNumber` (int, optional, default: 1): Page number
- `pageSize` (int, optional, default: 50): Items per page

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "items": [
      {
        "userId": "650e8400-e29b-41d4-a716-446655440001",
        "userName": "Jane Smith",
        "totalPoints": 1250,
        "level": 4,
        "rank": "Master",
        "badgeCount": 4,
        "questionsAsked": 15,
        "answersProvided": 48,
        "acceptedAnswers": 12
      },
      {
        "userId": "550e8400-e29b-41d4-a716-446655440000",
        "userName": "John Doe",
        "totalPoints": 850,
        "level": 3,
        "rank": "Expert",
        "badgeCount": 2,
        "questionsAsked": 22,
        "answersProvided": 31,
        "acceptedAnswers": 5
      }
    ],
    "pageNumber": 1,
    "totalPages": 3,
    "totalCount": 125,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

**Note:** Leaderboard results are cached for 5 minutes for performance.

---

### Get Current User's Reputation
Retrieve the authenticated user's reputation, level, rank, and badges.

**Endpoint:** `GET /api/qa/reputation`

**Authentication:** Required

**Response:** `200 OK`
```json
{
  "isSuccess": true,
  "data": {
    "totalPoints": 850,
    "level": 3,
    "rank": "Expert",
    "questionsAsked": 22,
    "answersProvided": 31,
    "acceptedAnswers": 5,
    "badges": [
      {
        "id": "9fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Contributor",
        "description": "Earned 100 total points",
        "badgeType": "Contributor",
        "iconUrl": "/badges/contributor.png",
        "earnedAt": "2024-02-15T10:30:00Z"
      },
      {
        "id": "afa85f64-5717-4562-b3fc-2c963f66afa7",
        "name": "Expert",
        "description": "Earned 500 total points",
        "badgeType": "Expert",
        "iconUrl": "/badges/expert.png",
        "earnedAt": "2024-02-28T14:20:00Z"
      }
    ]
  }
}
```

---

### Question Categories

The following categories are available for questions:

| Value | Category | Description |
|-------|----------|-------------|
| 0 | General | General car sharing questions |
| 1 | Maintenance | Vehicle maintenance and care |
| 2 | BuyingGuide | Advice on purchasing cars for sharing |
| 3 | Troubleshooting | Problem diagnosis and solutions |
| 4 | Insurance | Insurance-related questions |
| 5 | Legal | Legal aspects of car sharing |
| 6 | Safety | Safety tips and best practices |
| 7 | Technology | Tech features and integrations |
| 8 | CostSharing | Cost management and sharing |
| 9 | BestPractices | Community best practices |

---

### Reputation System

#### Point Awards

| Action | Points |
|--------|--------|
| Ask a question | +5 |
| Provide an answer | +10 |
| Question receives upvote | +5 (to author) |
| Question receives downvote | -2 (to author) |
| Answer receives upvote | +10 (to author) |
| Answer receives downvote | -5 (to author) |
| Answer is accepted | +25 (to author) |

**Note:** User points never fall below zero.

#### Levels and Ranks

| Level | Rank | Points Required |
|-------|------|-----------------|
| 1 | Beginner | 0-99 |
| 2 | Contributor | 100-499 |
| 3 | Expert | 500-999 |
| 4 | Master | 1000-2499 |
| 5 | Legend | 2500+ |

#### Badges

| Badge | Requirement |
|-------|-------------|
| Contributor | Reach 100 total points |
| Expert | Reach 500 total points |
| Master | Reach 1000 total points |
| Problem Solver | Have 10 accepted answers |
| Great Question | Question receives 50 upvotes |
| Great Answer | Answer receives 50 upvotes |

**Note:** Each badge is awarded only once per user.

---

### Q&A Error Codes

| Error Code | Description |
|------------|-------------|
| QA_QUESTION_NOT_FOUND | The specified question does not exist |
| QA_ANSWER_NOT_FOUND | The specified answer does not exist |
| QA_UNAUTHORIZED_DELETE | User does not have permission to delete this content |
| QA_UNAUTHORIZED_ACCEPT | Only the question author can accept answers |
| QA_CANNOT_VOTE_OWN | Users cannot vote on their own content |
| QA_DUPLICATE_ANSWER | User has already answered this question |
| QA_ALREADY_SOLVED | Question already has an accepted answer |
| QA_VALIDATION_ERROR | Input validation failed |

---

## Support

For API support, contact:
- Email: support@communitycar.com
- Documentation: https://api.communitycar.com/docs
- Swagger UI: https://api.communitycar.com/swagger
