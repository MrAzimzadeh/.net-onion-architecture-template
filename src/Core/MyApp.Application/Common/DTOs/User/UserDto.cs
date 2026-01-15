namespace MyApp.Application.Common.DTOs;

/// <summary>
/// User DTO for responses
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string EmailAddress { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();

    public UserDto(Guid id, string firstName, string lastName, string emailAddress, string? phoneNumber, bool isActive, List<string> roles)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
        IsActive = isActive;
        Roles = roles;
    }

    public UserDto(Guid id)
    {
        Id = id;
    }


    public UserDto()
    {
    }
}
