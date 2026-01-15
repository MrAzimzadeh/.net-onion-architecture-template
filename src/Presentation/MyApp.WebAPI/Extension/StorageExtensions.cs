using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.Common.Enums;
using MyApp.Application.Common.Interfaces.Storage;
using MyApp.Application.Common.Interfaces.Storage.Azure;
using MyApp.Application.Common.Interfaces.Storage.Local;
using MyApp.Application.Common.Interfaces.Storage.Minio;
using MyApp.Application.Common.Services.Storage;
using MyApp.Infrastructure.Storage.Azure;
using MyApp.Infrastructure.Storage.Local;
using MyApp.Infrastructure.Storage.Minio;

namespace MyApp.WebAPI.Extension;

public static class StorageExtensions
{
    public static void AddStorage<T>(this IServiceCollection services)
        where T : Application.Common.Services.Storage.Storage, IStorage
    {
        services.AddScoped<IStorage, T>();
    }

    public static void AddStorage(this IServiceCollection services, StorageType storageType)
    {
        services.AddScoped<IStorageService, StorageService>();

        switch (storageType)
        {
            case StorageType.Local:
                services.AddScoped<ILocalStorage, LocalStorage>();
                services.AddScoped<IStorage>(provider => provider.GetRequiredService<ILocalStorage>());
                break;
            case StorageType.Azure:
                services.AddScoped<IAzureStorage, AzureStorage>();
                services.AddScoped<IStorage>(provider => provider.GetRequiredService<IAzureStorage>());
                break;
            case StorageType.MinIO:
                services.AddScoped<IMinioStorage, MinioStorage>();
                services.AddScoped<IStorage>(provider => provider.GetRequiredService<IMinioStorage>());
                break;
            default:
                services.AddScoped<ILocalStorage, LocalStorage>();
                services.AddScoped<IStorage>(provider => provider.GetRequiredService<ILocalStorage>());
                break;
        }
    }
}
