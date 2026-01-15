namespace MyApp.Application.Common.Interfaces.Triggers;

/// <summary>
/// Application-level trigger logic for entity changes
/// </summary>
public interface IEntityTrigger
{
    string EntityName { get; }
    Task OnAddAsync(Guid entityId);
    Task OnUpdateAsync(Guid entityId);
    Task OnDeleteAsync(Guid entityId);
}
