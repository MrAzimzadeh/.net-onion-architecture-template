using MediatR;
using MyApp.Application.Common.Interfaces;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace MyApp.Application.Features.Auth.Commands.Register;

/// <summary>
/// Handler for RegisterCommand
/// </summary>
public class RegisterCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user already exists
            var existingUser = await unitOfWork.UserReads.GetByEmailAsync(request.EmailAddress, cancellationToken);
            if (existingUser != null)
                return Result.Failure<Guid>("User with this email already exists");

            // Generate password hash and salt
            var (passwordHash, passwordSalt) = HashPassword(request.Password);

            // Create user
            var user = new User
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.Empty, // System generated
                CreationAt = DateTime.UtcNow,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                PhoneNumber = request.PhoneNumber ?? string.Empty,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = true
            };

            // Save user
            await unitOfWork.UserWrites.AddAsync(user, cancellationToken);

            return Result.Success(user.Id, "User registered successfully");
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>("Failed to register user", ex.Message);
        }
    }

    private (string hash, string salt) HashPassword(string password)
    {
        // Generate salt
        var saltBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        var salt = Convert.ToBase64String(saltBytes);

        // Generate hash
        using var hmac = new HMACSHA512(saltBytes);
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hash = Convert.ToBase64String(hashBytes);

        return (hash, salt);
    }
}
