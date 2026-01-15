using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.Interfaces.Storage.Local;
using System.IO;

namespace MyApp.Infrastructure.Storage.Local;

public class LocalStorage(IWebHostEnvironment webHostEnvironment) : Application.Common.Services.Storage.Storage, ILocalStorage
{
    private string GetStorageBasePath()
    {
        if (!string.IsNullOrEmpty(webHostEnvironment?.WebRootPath))
            return webHostEnvironment.WebRootPath;

        return Path.Combine(webHostEnvironment?.ContentRootPath ?? AppContext.BaseDirectory, "uploads");
    }

    public async Task DeleteAsync(string path, string fileName)
    {
        string fullPath = Path.Combine(GetStorageBasePath(), path, fileName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        await Task.CompletedTask;
    }

    public List<string> GetFiles(string path)
    {
        string fullPath = Path.Combine(GetStorageBasePath(), path);
        if (!Directory.Exists(fullPath))
            return new List<string>();

        DirectoryInfo directory = new(fullPath);
        return directory.GetFiles().Select(f => f.Name).ToList();
    }

    public bool HasFile(string path, string fileName)
    {
        string fullPath = Path.Combine(GetStorageBasePath(), path, fileName);
        return File.Exists(fullPath);
    }

    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
    {
        string uploadPath = Path.Combine(GetStorageBasePath(), path);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        List<(string fileName, string pathOrContainerName)> datas = new();
        foreach (IFormFile file in files)
        {
            if (file.Length == 0) continue;

            string fileNewName = await FileRenameAsync(uploadPath, file.FileName, HasFile);
            string fullPath = Path.Combine(uploadPath, fileNewName);

            await using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();

            datas.Add((fileNewName, $"{path}/{fileNewName}"));
        }

        return datas;
    }

    public async Task<Stream> GetObjectStreamAsync(string path, string fileName)
    {
        string fullPath = Path.Combine(GetStorageBasePath(), path, fileName);
        if (!File.Exists(fullPath)) throw new FileNotFoundException($"File not found: {fullPath}");

        return await Task.FromResult(new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read));
    }

    public async Task<byte[]> GetObjectBytesAsync(string path, string fileName)
    {
        string fullPath = Path.Combine(GetStorageBasePath(), path, fileName);
        if (!File.Exists(fullPath)) throw new FileNotFoundException($"File not found: {fullPath}");

        return await File.ReadAllBytesAsync(fullPath);
    }
}
