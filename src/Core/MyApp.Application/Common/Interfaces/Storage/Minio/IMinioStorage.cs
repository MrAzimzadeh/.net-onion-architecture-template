using MyApp.Application.Common.Interfaces.Storage;

namespace MyApp.Application.Common.Interfaces.Storage.Minio;

public interface IMinioStorage : IStorage
{
    Task<string> GetPresignedUrlAsync(string pathOrContainerName, string fileName);
}
