using System.Security.Cryptography;
using System.Text;
using MyApp.Application.Common.Interfaces.Auth;

namespace MyApp.Infrastructure.Helpers;

/// <summary>
/// Password hashing and verification helper
/// </summary>
public class PasswordHandler : IPasswordHandler
{
    /// <summary>
    /// Creates a password hash with salt
    /// </summary>
    public (string PasswordHash, string PasswordSalt) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        var saltBytes = hmac.Key;
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return (
            PasswordHash: Convert.ToBase64String(hashBytes),
            PasswordSalt: Convert.ToBase64String(saltBytes)
        );
    }

    /// <summary>
    /// Verifies a password against stored hash and salt
    /// </summary>
    public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(storedHash);
    }

    /// <summary>
    /// Verifies a password against base64 encoded hash and salt
    /// </summary>
    public bool VerifyPassword(string password, string storedHashBase64, string storedSaltBase64)
    {
        var storedHash = Convert.FromBase64String(storedHashBase64);
        var storedSalt = Convert.FromBase64String(storedSaltBase64);
        return VerifyPassword(password, storedHash, storedSalt);
    }
}
