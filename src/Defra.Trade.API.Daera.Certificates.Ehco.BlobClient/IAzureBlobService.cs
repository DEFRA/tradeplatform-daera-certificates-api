// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;


/// <summary>
/// Interface allowing to interact with blobs in an Azure Blob Storage.
/// </summary>
public interface IAzureBlobService
{
    /// <summary>
    /// Check storage account health status.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HealthCheckResult> CheckStorageAccountHealth(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a blob from specified container along with blob options.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="blobName"></param>
    /// <returns></returns>
    Task<GetBlobResult> GetBlobWithOptionsAsync(string container, string blobName);
}
