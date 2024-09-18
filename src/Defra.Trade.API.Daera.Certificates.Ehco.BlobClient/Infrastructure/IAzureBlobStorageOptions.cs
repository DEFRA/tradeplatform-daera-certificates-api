// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;

public interface IAzureBlobStorageOptions
{
    /// <summary>
    /// Connection String to the storage account.
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Tenant id of the storage account.
    /// </summary>
    public string TenantId { get; }

    /// <summary>
    /// Client id of the storage account.
    /// </summary>
    public string ClientId { get; }

    /// <summary>
    /// Client secret of the storage account.
    /// </summary>
    public string ClientSecret { get; }

    /// <summary>
    /// Storage Account Name of the storage account.
    /// </summary>
    public string StorageAccountName { get; }

    /// <summary>Containers to be used.</summary>
    public Dictionary<string, string> Containers { get; init; }

    /// <summary>
    /// Settings section.
    /// </summary>
    public string SectionName { get; }

    /// <summary>Returns a container value by key.</summary>
    /// <param name="container."></param>
    /// <returns>Container name.</returns>
    public string GetContainer(string container);
}