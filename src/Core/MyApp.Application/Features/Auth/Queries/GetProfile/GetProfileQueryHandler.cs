using MediatR;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Queries.GetProfile;

/// <summary>
/// Handler for GetProfileQuery
/// </summary>
public class GetProfileQueryHandler(IUnitOfWork unitOfWork, IAuthHelper authHelper, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetProfileQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var userId = authHelper.GetUserId(httpContextAccessor.HttpContext);

            // Get user
            var user = await unitOfWork.UserReads.GetByIdAsync(userId, cancellationToken);

            if (user == null)
                return Result.Failure<UserDto>("User not found");

            // Get user roles
            var roles = await unitOfWork.UserReads.GetUserRolesAsync(userId, cancellationToken);

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Roles = roles.Select(r => r.Name).ToList()
            };

            return Result.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserDto>("Failed to get profile", ex.Message);
        }
    }
}
