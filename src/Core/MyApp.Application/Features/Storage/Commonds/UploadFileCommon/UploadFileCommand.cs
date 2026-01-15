using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Domain.Common;


namespace MyApp.Application.Features.Storage.Commonds.UploadFileCommon;

public record UploadFileCommand([FromQuery] string Path, [FromForm] IFormFileCollection Files) : IRequest<Result>;