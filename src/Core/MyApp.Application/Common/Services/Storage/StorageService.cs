using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.Interfaces.Storage;

namespace MyApp.Application.Common.Services.Storage;

public class StorageService(IStorage storage) : IStorageService
{
    public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainer, IFormFileCollection files)
        => storage.UploadAsync(pathOrContainer, files);

    public Task DeleteAsync(string pathOrContainer, string fileName)
        => storage.DeleteAsync(pathOrContainer, fileName);

    public List<string> GetFiles(string pathOrContainer)
        => storage.GetFiles(pathOrContainer);

    public bool HasFile(string pathOrContainer, string fileName)
        => storage.HasFile(pathOrContainer, fileName);

    public Task<Stream> GetObjectStreamAsync(string pathOrContainer, string fileName)
        => storage.GetObjectStreamAsync(pathOrContainer, fileName);

    public Task<byte[]> GetObjectBytesAsync(string pathOrContainer, string fileName)
        => storage.GetObjectBytesAsync(pathOrContainer, fileName);

    public string StorageName => storage.GetType().Name;
}
