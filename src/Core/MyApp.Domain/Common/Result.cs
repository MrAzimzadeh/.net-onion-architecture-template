using System.Net;

namespace MyApp.Domain.Common;

/// <summary>
/// Base result class for CQRS operations
/// </summary>
public class Result
{
    public bool IsSuccess { get; protected set; }
    public string Message { get; protected set; } = string.Empty;
    public string[] Errors { get; protected set; } = Array.Empty<string>();
    public HttpStatusCode HttpStatusCode { get; protected set; }
    public string? ErrorCode { get; protected set; }
    public string? TraceId { get; protected set; }
    public int StatusCode { get; protected set; }


    protected Result(bool isSuccess, string message, string[] errors)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors;
        HttpStatusCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        StatusCode = isSuccess ? 0 : 1;
        ErrorCode = isSuccess ? null : "ERR001";
    }
    protected Result(bool isSuccess, string message, string[] errors, HttpStatusCode httpStatusCode, int statusCode, string? errorCode)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors;
        HttpStatusCode = httpStatusCode;
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }





    public static Result Success(string message = "Operation successful")
        => new(true, message, Array.Empty<string>());

    public static Result Success(HttpStatusCode httpStatusCode = HttpStatusCode.OK, int statusCode = 0, string message = "Operation successful")
        => new(
            isSuccess: true,
            message: message,
            errors: Array.Empty<string>(),
            httpStatusCode: httpStatusCode,
            statusCode: statusCode,
            errorCode: null);



    public static Result Failure(string message, params string[] errors)
        => new(false, message, errors);
    public static Result Failure(HttpStatusCode httpStatusCode, int statusCode, string errorCode, string message, params string[] errors)
        => new(
            isSuccess: false,
            message: message,
            errors: errors,
            httpStatusCode: httpStatusCode,
            statusCode: statusCode,
            errorCode: errorCode);


    public static Result<T> Success<T>(T data, string message = "Operation successful")
        => new(true, message, Array.Empty<string>(), data);

    public static Result<T> Failure<T>(string message, params string[] errors)
        => new(false, message, errors, default);
}

/// <summary>
/// Generic result class with data
/// </summary>
public class Result<T>(bool isSuccess, string message, string[] errors, T? data) : Result(isSuccess, message, errors)
{
    public T? Data { get; private set; } = data;
}
