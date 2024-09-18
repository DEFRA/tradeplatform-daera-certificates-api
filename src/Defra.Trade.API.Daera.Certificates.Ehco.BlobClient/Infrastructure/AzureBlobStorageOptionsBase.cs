// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;

public class AzureBlobStorageOptionsBase : IAzureBlobStorageOptions
{
    /// <inheritdoc/>
    public string ConnectionString { get; init; } = string.Empty;

    /// <inheritdoc/>
    public string TenantId { get; init; } = string.Empty;

    /// <inheritdoc/>
    public string ClientId { get; init; } = string.Empty;

    /// <inheritdoc/>
    public string ClientSecret { get; init; } = string.Empty;

    /// <inheritdoc/>
    public string StorageAccountName { get; init; } = string.Empty;

    /// <inheritdoc/>
    public Dictionary<string, string> Containers { get; init; } = new();

    /// <inheritdoc/>
    public virtual string SectionName { get; } = string.Empty;

    public string SourceContainer => GetContainer(nameof(StorageContainerType.Source));

    /// <inheritdoc/>
    public string GetContainer(string container)
    {
        return Containers.TryGetValue(container, out string? containerValue) ? containerValue : string.Empty;
    }
}