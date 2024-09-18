// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Diagnostics.CodeAnalysis;
using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Extensions;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;

/// <summary>
/// Generic service that allows the interaction with an Azure Blob Storage account.
/// </summary>
/// <typeparam name="TOptions">Configuration options.</typeparam>
[ExcludeFromCodeCoverage(Justification = "Services tested by passing actual options object.")]
public class AzureBlobService<TOptions> : IAzureBlobService<TOptions>
    where TOptions : class, IAzureBlobStorageOptions
{
    private readonly ILogger<AzureBlobService<TOptions>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureBlobService{TOptions}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public AzureBlobService(
        ILogger<AzureBlobService<TOptions>> logger,
        IOptions<TOptions> options)
    {
        _logger = logger;
        Options = options.Value;
    }

    /// <inheritdoc/>
    public TOptions Options { get; }

    public async Task<HealthCheckResult> CheckStorageAccountHealth(CancellationToken cancellationToken = default)
    {
        try
        {
            var clientSecretCredential = new ClientSecretCredential(Options.TenantId, Options.ClientId, Options.ClientSecret);
            var blobServiceClient = new BlobServiceClient(new Uri($"https://{Options.StorageAccountName}.blob.core.windows.net"), clientSecretCredential);
            foreach (var blobContainerClient in Options.Containers.Values.Select(container => blobServiceClient.GetBlobContainerClient(container)))
            {
                var containerExists = await blobContainerClient.ExistsAsync(cancellationToken);
                if (!containerExists)
                {
                    return HealthCheckResult.Unhealthy();
                }
            }
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }

    public async Task<GetBlobResult> GetBlobWithOptionsAsync(string container, string blobName)
    {
        _logger.BlobRetrievalStart(blobName, container);

        var blobServiceClient = CreateBlobServiceClient();
        try
        {
            var containerClient = CreateBlobContainerClient(container, blobServiceClient);
            var blobResult = await GetBlobWithOptionsAsync(blobName, containerClient);
            return blobResult;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            _logger.BlobNotFound(ex, blobName, container);
            throw new FileNotFoundException($"Blob: ${blobName} from ${container} not found");
        }
        catch (Exception ex)
        {
            _logger.BlobRetrievalError(ex, blobName, container);
            throw;
        }
    }

    private static BlobContainerClient CreateBlobContainerClient(
        string container, BlobServiceClient blobServiceClient)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(container);
        return containerClient;
    }

    private static async Task<GetBlobResult> GetBlobWithOptionsAsync(string blobName, BlobContainerClient containerClient)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        var blobContentResult = await blobClient.DownloadStreamingAsync();

        return new GetBlobResult
        {
            ContentType = blobContentResult.Value.Details.ContentType,
            FileContent = blobContentResult.Value.Content
        };
    }

    private BlobServiceClient CreateBlobServiceClient()
    {
        if (!string.IsNullOrWhiteSpace(Options.ConnectionString))
        {
            return new BlobServiceClient(Options.ConnectionString);
        }
        else
        {
            var clientSecretCredential = new ClientSecretCredential(Options.TenantId, Options.ClientId, Options.ClientSecret);
            return new BlobServiceClient(new Uri($"https://{Options.StorageAccountName}.blob.core.windows.net"), clientSecretCredential);
        }
    }
}