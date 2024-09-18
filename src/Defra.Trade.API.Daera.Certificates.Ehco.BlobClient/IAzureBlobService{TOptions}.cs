// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;

/// <summary>Interface allowing to interact with blobs in an Azure Blob Storage.</summary>
/// <typeparam name="TOptions">Options for configuring the blob storage.</typeparam>
public interface IAzureBlobService<out TOptions> : IAzureBlobService
    where TOptions : class, IAzureBlobStorageOptions
{
    /// <summary>Exposes the service options.</summary>
    TOptions Options { get; }
}
