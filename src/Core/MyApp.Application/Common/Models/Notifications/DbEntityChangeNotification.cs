using MediatR;

namespace MyApp.Application.Common.Models.Notifications;

/// <summary>
/// Integration notification representing a change in the database
/// </summary>
public record DbEntityChangeNotification(string Table, Guid Id, string Operation) : INotification;
