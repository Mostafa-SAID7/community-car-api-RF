namespace CommunityCarApi.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public string? ErrorCode { get; set; }
    public Dictionary<string, string[]>? ValidationErrors { get; set; }

    public static Result<T> Success(T data) => new() 
    { 
        IsSuccess = true, 
        Data = data 
    };

    public static Result<T> Failure(string error, string? errorCode = null) => new() 
    { 
        IsSuccess = false, 
        Error = error,
        ErrorCode = errorCode 
    };

    public static Result<T> ValidationFailure(Dictionary<string, string[]> validationErrors) => new()
    {
        IsSuccess = false,
        Error = "Validation failed",
        ErrorCode = "VALIDATION_ERROR",
        ValidationErrors = validationErrors
    };
}

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public string? ErrorCode { get; set; }
    public Dictionary<string, string[]>? ValidationErrors { get; set; }

    public static Result Success() => new() 
    { 
        IsSuccess = true 
    };

    public static Result Failure(string error, string? errorCode = null) => new() 
    { 
        IsSuccess = false, 
        Error = error,
        ErrorCode = errorCode 
    };

    public static Result ValidationFailure(Dictionary<string, string[]> validationErrors) => new()
    {
        IsSuccess = false,
        Error = "Validation failed",
        ErrorCode = "VALIDATION_ERROR",
        ValidationErrors = validationErrors
    };
}
