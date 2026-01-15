namespace MyApp.Application.Common.DTOs;

/// <summary>
/// Role DTO
/// </summary>
public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Policies { get; set; } = new();
}
