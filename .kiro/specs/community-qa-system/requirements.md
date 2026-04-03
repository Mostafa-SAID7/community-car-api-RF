# Requirements Document

## Introduction

The Community Q&A System is a gamified knowledge-sharing platform that enables users of the CommunityCarApi to ask questions about car sharing, receive answers from the community, and build reputation through participation. The system incentivizes quality contributions through points, badges, and leaderboards while maintaining content quality through voting and answer acceptance mechanisms.

## Glossary

- **QA_System**: The Community Q&A System component
- **Question**: A user-submitted inquiry about car sharing topics
- **Answer**: A user-submitted response to a Question
- **Vote**: A user action indicating approval (upvote) or disapproval (downvote) of content
- **Accepted_Answer**: An Answer marked as the solution by the Question author
- **Points**: Numeric rewards earned through participation in the QA_System
- **Badge**: An achievement award granted for reaching specific milestones
- **Level**: A user's rank calculated from accumulated Points
- **Leaderboard**: A ranked list of users based on Points or other metrics
- **Question_Author**: The user who created a specific Question
- **Answer_Author**: The user who created a specific Answer
- **Authenticated_User**: A user who has successfully logged in to the system
- **Category**: A classification tag for Questions (e.g., Maintenance, BuyingGuide, Troubleshooting)
- **Trending_Question**: A Question with high recent activity (views, votes, answers)

## Requirements

### Requirement 1: Ask Questions

**User Story:** As an Authenticated_User, I want to ask questions about car sharing, so that I can get help from the community.

#### Acceptance Criteria

1. WHEN an Authenticated_User submits a Question with title and content, THE QA_System SHALL create the Question with a unique identifier
2. THE QA_System SHALL require Question titles to be between 10 and 200 characters
3. THE QA_System SHALL require Question content to be between 20 and 5000 characters
4. WHEN a Question is created, THE QA_System SHALL assign it to a Category
5. WHEN a Question is created, THE QA_System SHALL initialize vote count, answer count, and view count to zero
6. WHEN a Question is created, THE QA_System SHALL set IsSolved to false
7. THE QA_System SHALL allow optional tags (comma-separated) with each tag between 2 and 30 characters

### Requirement 2: Answer Questions

**User Story:** As an Authenticated_User, I want to answer questions, so that I can help others and earn reputation.

#### Acceptance Criteria

1. WHEN an Authenticated_User submits an Answer to an existing Question, THE QA_System SHALL create the Answer with a unique identifier
2. THE QA_System SHALL require Answer content to be between 20 and 5000 characters
3. WHEN an Answer is created, THE QA_System SHALL initialize vote count to zero and IsAccepted to false
4. WHEN an Answer is created, THE QA_System SHALL increment the Question's answer count by one
5. IF a Question does not exist, THEN THE QA_System SHALL return an error indicating the Question was not found
6. THE QA_System SHALL allow multiple Answers per Question from different users
7. THE QA_System SHALL allow a single user to provide only one Answer per Question

### Requirement 3: Vote on Questions

**User Story:** As an Authenticated_User, I want to vote on questions, so that I can indicate which questions are valuable.

#### Acceptance Criteria

1. WHEN an Authenticated_User votes on a Question, THE QA_System SHALL record the vote (upvote or downvote)
2. WHEN an upvote is recorded, THE QA_System SHALL increment the Question's vote count by one
3. WHEN a downvote is recorded, THE QA_System SHALL decrement the Question's vote count by one
4. WHEN a user changes their vote, THE QA_System SHALL update the vote count accordingly (from upvote to downvote changes count by -2, from downvote to upvote changes count by +2)
5. WHEN a user removes their vote, THE QA_System SHALL adjust the vote count to reflect the removal
6. THE QA_System SHALL prevent users from voting on their own Questions
7. THE QA_System SHALL allow each user to cast only one vote per Question

### Requirement 4: Vote on Answers

**User Story:** As an Authenticated_User, I want to vote on answers, so that I can highlight helpful responses.

#### Acceptance Criteria

1. WHEN an Authenticated_User votes on an Answer, THE QA_System SHALL record the vote (upvote or downvote)
2. WHEN an upvote is recorded, THE QA_System SHALL increment the Answer's vote count by one
3. WHEN a downvote is recorded, THE QA_System SHALL decrement the Answer's vote count by one
4. WHEN a user changes their vote, THE QA_System SHALL update the vote count accordingly (from upvote to downvote changes count by -2, from downvote to upvote changes count by +2)
5. WHEN a user removes their vote, THE QA_System SHALL adjust the vote count to reflect the removal
6. THE QA_System SHALL prevent users from voting on their own Answers
7. THE QA_System SHALL allow each user to cast only one vote per Answer

### Requirement 5: Accept Answers

**User Story:** As a Question_Author, I want to mark an answer as accepted, so that I can indicate which answer solved my problem.

#### Acceptance Criteria

1. WHEN a Question_Author marks an Answer as accepted, THE QA_System SHALL set the Answer's IsAccepted property to true
2. WHEN an Answer is accepted, THE QA_System SHALL set the Question's IsSolved property to true
3. WHEN a Question_Author accepts a different Answer, THE QA_System SHALL set the previously Accepted_Answer's IsAccepted property to false
4. THE QA_System SHALL allow only the Question_Author to accept Answers for their Question
5. THE QA_System SHALL allow only one Accepted_Answer per Question at any time
6. IF a user who is not the Question_Author attempts to accept an Answer, THEN THE QA_System SHALL return an authorization error

### Requirement 6: Delete Questions

**User Story:** As a Question_Author or Admin, I want to delete questions, so that I can remove inappropriate or outdated content.

#### Acceptance Criteria

1. WHEN a Question_Author requests deletion of their Question, THE QA_System SHALL perform a soft delete by setting IsDeleted to true
2. WHEN an Admin requests deletion of any Question, THE QA_System SHALL perform a soft delete by setting IsDeleted to true
3. WHEN a Question is deleted, THE QA_System SHALL also soft delete all associated Answers
4. IF a user who is neither the Question_Author nor an Admin attempts to delete a Question, THEN THE QA_System SHALL return an authorization error
5. WHEN a Question is soft deleted, THE QA_System SHALL exclude it from query results by default

### Requirement 7: Delete Answers

**User Story:** As an Answer_Author or Admin, I want to delete answers, so that I can remove incorrect or inappropriate responses.

#### Acceptance Criteria

1. WHEN an Answer_Author requests deletion of their Answer, THE QA_System SHALL perform a soft delete by setting IsDeleted to true
2. WHEN an Admin requests deletion of any Answer, THE QA_System SHALL perform a soft delete by setting IsDeleted to true
3. WHEN an Accepted_Answer is deleted, THE QA_System SHALL set the Question's IsSolved property to false
4. WHEN an Answer is deleted, THE QA_System SHALL decrement the Question's answer count by one
5. IF a user who is neither the Answer_Author nor an Admin attempts to delete an Answer, THEN THE QA_System SHALL return an authorization error
6. WHEN an Answer is soft deleted, THE QA_System SHALL exclude it from query results by default

### Requirement 8: View Questions with Filtering

**User Story:** As a user, I want to view and filter questions, so that I can find relevant content.

#### Acceptance Criteria

1. WHEN a user requests Questions, THE QA_System SHALL return a paginated list of non-deleted Questions
2. WHERE a Category filter is provided, THE QA_System SHALL return only Questions matching that Category
3. WHERE a search term is provided, THE QA_System SHALL return Questions where the title or content contains the search term
4. WHERE a tag filter is provided, THE QA_System SHALL return Questions containing that tag
5. WHERE an IsSolved filter is provided, THE QA_System SHALL return Questions matching the solved status
6. THE QA_System SHALL support sorting by CreatedAt (newest/oldest), VoteCount (highest/lowest), AnswerCount (most/least), and ViewCount (most/least)
7. THE QA_System SHALL increment a Question's ViewCount by one when the Question details are retrieved

### Requirement 9: View Trending Questions

**User Story:** As a user, I want to see trending questions, so that I can discover popular discussions.

#### Acceptance Criteria

1. WHEN a user requests trending Questions, THE QA_System SHALL return Questions with high recent activity
2. THE QA_System SHALL calculate trending score based on ViewCount, VoteCount, and AnswerCount from the last 7 days
3. THE QA_System SHALL return trending Questions sorted by trending score in descending order
4. THE QA_System SHALL limit trending Questions to a maximum of 20 results
5. THE QA_System SHALL exclude deleted Questions from trending results

### Requirement 10: View User's Own Questions

**User Story:** As an Authenticated_User, I want to view my own questions, so that I can track my inquiries and their status.

#### Acceptance Criteria

1. WHEN an Authenticated_User requests their Questions, THE QA_System SHALL return all Questions created by that user
2. THE QA_System SHALL include both solved and unsolved Questions in the results
3. THE QA_System SHALL return Questions sorted by CreatedAt in descending order (newest first)
4. THE QA_System SHALL support pagination for the user's Questions
5. THE QA_System SHALL exclude soft-deleted Questions from the results

### Requirement 11: Calculate Points for Participation

**User Story:** As an Authenticated_User, I want to earn points for my contributions, so that I can build reputation in the community.

#### Acceptance Criteria

1. WHEN a user creates a Question, THE QA_System SHALL award 5 Points to the user
2. WHEN a user creates an Answer, THE QA_System SHALL award 10 Points to the user
3. WHEN a user's Question receives an upvote, THE QA_System SHALL award 5 Points to the Question_Author
4. WHEN a user's Question receives a downvote, THE QA_System SHALL deduct 2 Points from the Question_Author
5. WHEN a user's Answer receives an upvote, THE QA_System SHALL award 10 Points to the Answer_Author
6. WHEN a user's Answer receives a downvote, THE QA_System SHALL deduct 5 Points from the Answer_Author
7. WHEN a user's Answer is accepted, THE QA_System SHALL award 25 Points to the Answer_Author
8. THE QA_System SHALL ensure a user's total Points never falls below zero

### Requirement 12: Award Badges

**User Story:** As an Authenticated_User, I want to earn badges for achievements, so that I can showcase my expertise.

#### Acceptance Criteria

1. WHEN a user reaches 100 total Points, THE QA_System SHALL award the "Contributor" Badge
2. WHEN a user reaches 500 total Points, THE QA_System SHALL award the "Expert" Badge
3. WHEN a user reaches 1000 total Points, THE QA_System SHALL award the "Master" Badge
4. WHEN a user has 10 Accepted_Answers, THE QA_System SHALL award the "Problem Solver" Badge
5. WHEN a user's Question receives 50 upvotes, THE QA_System SHALL award the "Great Question" Badge
6. WHEN a user's Answer receives 50 upvotes, THE QA_System SHALL award the "Great Answer" Badge
7. THE QA_System SHALL award each Badge type only once per user
8. THE QA_System SHALL record the timestamp when each Badge is earned

### Requirement 13: Calculate User Level

**User Story:** As an Authenticated_User, I want my level to reflect my participation, so that I can see my progression.

#### Acceptance Criteria

1. WHEN a user has 0-99 Points, THE QA_System SHALL assign Level 1 with Rank "Beginner"
2. WHEN a user has 100-499 Points, THE QA_System SHALL assign Level 2 with Rank "Contributor"
3. WHEN a user has 500-999 Points, THE QA_System SHALL assign Level 3 with Rank "Expert"
4. WHEN a user has 1000-2499 Points, THE QA_System SHALL assign Level 4 with Rank "Master"
5. WHEN a user has 2500 or more Points, THE QA_System SHALL assign Level 5 with Rank "Legend"
6. WHEN a user's Points change, THE QA_System SHALL recalculate their Level and Rank
7. THE QA_System SHALL update Level and Rank immediately after Points are awarded or deducted

### Requirement 14: Display Leaderboard

**User Story:** As a user, I want to view the leaderboard, so that I can see top contributors.

#### Acceptance Criteria

1. WHEN a user requests the Leaderboard, THE QA_System SHALL return users ranked by total Points in descending order
2. THE QA_System SHALL include each user's total Points, Level, Rank, and Badge count in the Leaderboard
3. THE QA_System SHALL support pagination for the Leaderboard with a default page size of 50
4. THE QA_System SHALL allow filtering the Leaderboard by time period (all-time, monthly, weekly)
5. WHERE a time period filter is provided, THE QA_System SHALL calculate Points earned only within that period
6. THE QA_System SHALL exclude users with zero Points from the Leaderboard
7. THE QA_System SHALL cache Leaderboard results for 5 minutes to improve performance
