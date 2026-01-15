using MediatR;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.Interfaces;
using MyApp.Domain.Common;
using MyApp.Application.Common.Interfaces.Auth;

namespace MyApp.Application.Features.Auth.Commands.ChangePassword;

/// <summary>
/// Handler for ChangePasswordCommand
/// </summary>
public class ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IAuthHelper authHelper, IPasswordHandler passwordHandler) : IRequestHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var userId = authHelper.GetUserId(httpContextAccessor.HttpContext);

            // Get user
            var user = await unitOfWork.UserReads.GetByIdAsync(userId, cancellationToken);

            if (user == null)
                return Result.Failure("User not found");

            if (!user.IsActive)
                return Result.Failure("User account is deactivated");

            // Verify current password
            if (!passwordHandler.VerifyPassword(request.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                return Result.Failure("Current password is incorrect");

            // Validate new password matches confirmation
            if (request.NewPassword != request.ConfirmPassword)
                return Result.Failure("New password and confirmation do not match");

            // Create new password hash
            var (newPasswordHash, newPasswordSalt) = passwordHandler.CreatePasswordHash(request.NewPassword);

            user.PasswordHash = newPasswordHash;
            user.PasswordSalt = newPasswordSalt;

            await unitOfWork.UserWrites.UpdateAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success("Password changed successfully");
        }
        catch (Exception ex)
        {
            return Result.Failure("Failed to change password", ex.Message);
        }
    }
}
