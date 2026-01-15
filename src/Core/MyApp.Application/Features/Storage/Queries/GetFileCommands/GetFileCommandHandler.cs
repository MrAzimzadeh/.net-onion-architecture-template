using MediatR;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Storage.Local;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Storage.Queries.GetFileCommands;

public class GetFileCommandHandler(ILocalStorage localStorage) : IRequestHandler<GetFileCommand, Result<Stream>>
{
    public async Task<Result<Stream>> Handle(GetFileCommand request, CancellationToken cancellationToken)
    {
        var result = await localStorage.GetObjectStreamAsync(request.Path, request.FileName);
        return Result.Success(result);
    }
}