namespace MyApp.Application.Common.Interfaces.Auth;


/// <summary>
/// Password hashing and verification helper
/// </summary>
public interface IPasswordHandler
{
    /// <summary>
    /// Creates a password hash with salt
    /// </summary>
    (string PasswordHash, string PasswordSalt) CreatePasswordHash(string password);
    /// <summary>
    /// Verifies a password against stored hash and salt
    /// </summary>
    bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);

    /// <summary>
    /// Verifies a password against base64 encoded hash and salt
    /// </summary>
    bool VerifyPassword(string password, string storedHashBase64, string storedSaltBase64);

}