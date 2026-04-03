namespace CommunityCarApi.Application.Common;

public static class ErrorCodes
{
    // Authentication & Authorization
    public const string Unauthorized = "UNAUTHORIZED";
    public const string Forbidden = "FORBIDDEN";
    public const string InvalidCredentials = "INVALID_CREDENTIALS";
    public const string EmailAlreadyExists = "EMAIL_ALREADY_EXISTS";
    public const string UserNotFound = "USER_NOT_FOUND";
    public const string InvalidToken = "INVALID_TOKEN";

    // Validation
    public const string ValidationError = "VALIDATION_ERROR";
    public const string InvalidInput = "INVALID_INPUT";

    // Resources
    public const string NotFound = "NOT_FOUND";
    public const string AlreadyExists = "ALREADY_EXISTS";
    public const string Conflict = "CONFLICT";

    // Business Logic
    public const string CarNotAvailable = "CAR_NOT_AVAILABLE";
    public const string BookingConflict = "BOOKING_CONFLICT";
    public const string CannotCancelBooking = "CANNOT_CANCEL_BOOKING";
    public const string AlreadyReviewed = "ALREADY_REVIEWED";
    public const string NoCompletedBooking = "NO_COMPLETED_BOOKING";

    // Q&A System
    public const string QA_QUESTION_NOT_FOUND = "QA_QUESTION_NOT_FOUND";
    public const string QA_ANSWER_NOT_FOUND = "QA_ANSWER_NOT_FOUND";
    public const string QA_UNAUTHORIZED_DELETE = "QA_UNAUTHORIZED_DELETE";
    public const string QA_UNAUTHORIZED_ACCEPT = "QA_UNAUTHORIZED_ACCEPT";
    public const string QA_CANNOT_VOTE_OWN = "QA_CANNOT_VOTE_OWN";
    public const string QA_DUPLICATE_ANSWER = "QA_DUPLICATE_ANSWER";
    public const string QA_ALREADY_SOLVED = "QA_ALREADY_SOLVED";
    public const string QA_VALIDATION_ERROR = "QA_VALIDATION_ERROR";

    // System
    public const string InternalError = "INTERNAL_ERROR";
    public const string ServiceUnavailable = "SERVICE_UNAVAILABLE";
}
