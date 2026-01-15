using Microsoft.AspNetCore.Http;

namespace MyApp.Application.Common.Interfaces.Storage;

public interface IStorage
{
    Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files);
    Task DeleteAsync(string pathOrContainerName, string fileName);
    List<string> GetFiles(string pathOrContainerName);
    bool HasFile(string pathOrContainerName, string fileName);
    Task<Stream> GetObjectStreamAsync(string pathOrContainerName, string fileName);
    Task<byte[]> GetObjectBytesAsync(string pathOrContainerName, string fileName);
}
