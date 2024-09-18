﻿// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Concurrent;
using System.IO;
using System.Text;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Defra.Trade.API.Daera.Certificates.IntegrationTests.Helpers;

public class FakeBlobService<TOptions>(
    ILogger<FakeBlobService<TOptions>> logger,
    IOptions<TOptions> options) : IAzureBlobService<TOptions>
    where TOptions : class, IAzureBlobStorageOptions
{
    private readonly ILogger<FakeBlobService<TOptions>> _logger = logger;

    private readonly IDictionary<string, Dictionary<string, Stream>> _store
        = new ConcurrentDictionary<string, Dictionary<string, Stream>>();

    public TOptions Options { get; } = options.Value;

    public Task<HealthCheckResult> CheckStorageAccountHealth(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }

    public Task<GetBlobResult> GetBlobWithOptionsAsync(string container, string blobName)
    {
        string stubImage = "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";

        var stubImageStream = new MemoryStream(Encoding.UTF8.GetBytes(stubImage));
        _store.Add(container, new Dictionary<string, Stream>() { { blobName, stubImageStream } });
        _logger.LogInformation("Beginning get {BlobName} from {Container}", blobName, container);

        if (!_store.TryGetValue(container, out var containerStore))
        {
            throw new InvalidOperationException($"Invalid {container}");
        }

        if (!containerStore.TryGetValue(blobName, out var blobNameStore))
        {
            throw new FileNotFoundException($"Invalid {blobName}");
        }

        _logger.LogInformation("Found {BlobName} from {Container}", blobNameStore, container);

        return Task.FromResult(new GetBlobResult { FileContent = blobNameStore, ContentType = "image/jpeg" });
    }
}
