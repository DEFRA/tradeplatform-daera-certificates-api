// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;


public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBlobStorageServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterBlobStorage<GcDocumentBlobStorageOptions>(configuration);
        return services;
    }

    /// <summary>
    /// Helper for registering azure blob storage.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterBlobStorage<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class, IAzureBlobStorageOptions, new()
    {
        string sectionName = new TOptions().SectionName;
        services
            .Configure<TOptions>(configuration.GetSection(sectionName));

        services
            .AddTransient<IAzureBlobService<TOptions>, AzureBlobService<TOptions>>();
        return services;
    }
}
