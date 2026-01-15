namespace MyApp.Application.Common.Exceptions;

/// <summary>
/// Base exception for application specific exceptions
/// </summary>
public class AppException : Exception
{
    public string? ErrorCode { get; }
    public int StatusCode { get; }

    public AppException(string message, string? errorCode = null, int statusCode = 1)
        : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}
