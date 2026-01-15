using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MyApp.Application.Common.Interfaces.Storage.Azure;

namespace MyApp.Infrastructure.Storage.Azure;

public class AzureStorage(IConfiguration configuration) : Application.Common.Services.Storage.Storage, IAzureStorage
{
    private readonly BlobServiceClient _blobServiceClient = new(configuration["Storage:AzureConnectionString"]);

    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

        List<(string fileName, string pathOrContainerName)> datas = new();
        foreach (IFormFile file in files)
        {
            if (file.Length == 0) continue;

            string fileNewName = await FileRenameAsync(containerName, file.FileName, HasFile);
            BlobClient blobClient = containerClient.GetBlobClient(fileNewName);
            await blobClient.UploadAsync(file.OpenReadStream());
            datas.Add((fileNewName, $"{containerName}/{fileNewName}"));
        }

        return datas;
    }

    public async Task DeleteAsync(string container, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(container);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteAsync();
    }

    public List<string> GetFiles(string container)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(container);
        return containerClient.GetBlobs().Select(b => b.Name).ToList();
    }

    public bool HasFile(string container, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(container);
        return containerClient.GetBlobs().Any(b => b.Name == fileName);
    }

    public async Task<Stream> GetObjectStreamAsync(string container, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(container);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        MemoryStream memoryStream = new();
        await blobClient.DownloadToAsync(memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
    }

    public async Task<byte[]> GetObjectBytesAsync(string container, string fileName)
    {
        using var stream = await GetObjectStreamAsync(container, fileName);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}
