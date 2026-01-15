namespace MyApp.Application.Common.Interfaces.Storage;

public interface IStorageService : IStorage
{
    public string StorageName { get; }
}
