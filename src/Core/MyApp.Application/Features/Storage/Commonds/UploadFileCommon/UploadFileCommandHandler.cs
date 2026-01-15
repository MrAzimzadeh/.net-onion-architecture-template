using MediatR;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Storage.Local;
using MyApp.Application.Features.Storage.Commonds.UploadFileCommon;
using MyApp.Domain.Common;

public class UploadFileCommandHandler(ILocalStorage localStorage) : IRequestHandler<UploadFileCommand, Result>
{
    public async Task<Result> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var result = await localStorage.UploadAsync(request.Path, request.Files);
        return Result.Success(result);
    }
}