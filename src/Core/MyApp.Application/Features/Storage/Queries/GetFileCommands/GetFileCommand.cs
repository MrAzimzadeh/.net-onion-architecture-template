using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Storage.Queries.GetFileCommands;

public record GetFileCommand([FromQuery] string Path, [FromQuery] string FileName) : IRequest<Result<Stream>>;