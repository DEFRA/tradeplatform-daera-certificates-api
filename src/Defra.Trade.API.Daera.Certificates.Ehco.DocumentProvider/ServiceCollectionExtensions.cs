// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defra.Trade.API.Daera.Certificates.Ehco.DocumentProvider;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddGcDocumentProvider(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterBlobStorageServices(configuration);

        services.AddTransient<IGcDocumentProvider, GcDocumentProvider>();
        return services;
    }

}