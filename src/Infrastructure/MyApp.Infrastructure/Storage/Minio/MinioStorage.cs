using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using MyApp.Application.Common.Interfaces.Storage.Minio;
using System.IO;

namespace MyApp.Infrastructure.Storage.Minio;

public class MinioStorage(IMinioClient minioClient, IConfiguration configuration) : Application.Common.Services.Storage.Storage, IMinioStorage
{
    private readonly string _bucketName = configuration.GetValue<string>("MinIO:BucketName") ?? "default-bucket";

    private async Task EnsureHasBucketAsync()
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
        if (!await minioClient.BucketExistsAsync(bucketExistsArgs))
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
            await minioClient.MakeBucketAsync(makeBucketArgs);
        }
    }

    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
    {
        await EnsureHasBucketAsync();
        List<(string fileName, string pathOrContainerName)> uploadedFiles = new();

        foreach (IFormFile file in files)
        {
            if (file.Length == 0) continue;

            string fileNewName = await FileRenameAsync(pathOrContainerName, file.FileName, HasFile);
            string objectName = Path.Combine(pathOrContainerName, fileNewName).Replace("\\", "/");

            using var stream = file.OpenReadStream();
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithObjectSize(file.Length)
                .WithStreamData(stream)
                .WithContentType(file.ContentType);

            await minioClient.PutObjectAsync(putObjectArgs);
            uploadedFiles.Add((fileNewName, $"{pathOrContainerName}/{fileNewName}"));
        }

        return uploadedFiles;
    }

    public async Task DeleteAsync(string pathOrContainerName, string fileName)
    {
        await EnsureHasBucketAsync();
        string objectName = Path.Combine(pathOrContainerName, fileName).Replace("\\", "/");
        var removeObjectArgs = new RemoveObjectArgs().WithBucket(_bucketName).WithObject(objectName);
        await minioClient.RemoveObjectAsync(removeObjectArgs);
    }

    public List<string> GetFiles(string pathOrContainerName)
    {
        // MinIO listing is typically async/reactive, returning empty for simplicity in sync method
        return new List<string>();
    }

    public async Task<string> GetPresignedUrlAsync(string pathOrContainerName, string fileName)
    {
        string objectName = Path.Combine(pathOrContainerName, fileName).Replace("\\", "/");
        var presignedArgs = new PresignedGetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithExpiry(7 * 24 * 60 * 60);

        return await minioClient.PresignedGetObjectAsync(presignedArgs);
    }

    public async Task<Stream> GetObjectStreamAsync(string pathOrContainerName, string fileName)
    {
        await EnsureHasBucketAsync();
        string objectName = Path.Combine(pathOrContainerName, fileName).Replace("\\", "/");
        MemoryStream memoryStream = new();

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;
            });

        await minioClient.GetObjectAsync(getObjectArgs);
        return memoryStream;
    }

    public async Task<byte[]> GetObjectBytesAsync(string pathOrContainerName, string fileName)
    {
        using var stream = await GetObjectStreamAsync(pathOrContainerName, fileName);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public bool HasFile(string pathOrContainerName, string fileName)
    {
        try
        {
            string objectName = Path.Combine(pathOrContainerName, fileName).Replace("\\", "/");
            var statArgs = new StatObjectArgs().WithBucket(_bucketName).WithObject(objectName);
            var stat = minioClient.StatObjectAsync(statArgs).Result;
            return stat != null;
        }
        catch { return false; }
    }
}
