using System.Net;
using MyApp.Application.Common.Constants.ExceptionKeys;

namespace MyApp.Application.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message, string? errorCode = BaseExceptionConstants.NotFoundException)
        : base(message, errorCode, (int)HttpStatusCode.NotFound)
    {
    }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.", "NOT_FOUND", (int)HttpStatusCode.NotFound)
    {
    }
}
