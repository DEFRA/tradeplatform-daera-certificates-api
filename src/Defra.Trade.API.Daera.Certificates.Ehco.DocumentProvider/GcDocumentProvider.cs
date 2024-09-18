// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;

namespace Defra.Trade.API.Daera.Certificates.Ehco.DocumentProvider;

/// <inheritdoc />
public class GcDocumentProvider : IGcDocumentProvider
{
    private readonly IAzureBlobService<GcDocumentBlobStorageOptions> _gcBlobService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GcDocumentProvider"/> class.
    /// </summary>
    /// <param name="gcBlobService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GcDocumentProvider(IAzureBlobService<GcDocumentBlobStorageOptions> gcBlobService)
    {
        _gcBlobService = gcBlobService ?? throw new ArgumentNullException(nameof(gcBlobService));
    }

    /// <inheritdoc />
    public async Task<GetBlobResult> GetGcDocumentById(string blobName)
    {
        string blobNameFromUri = blobName;
        if (blobName.Contains(_gcBlobService.Options.SourceContainer))
        {
            blobNameFromUri = blobName.Split(_gcBlobService.Options.SourceContainer)[1];
        }
        var gcDocument = await _gcBlobService.GetBlobWithOptionsAsync(_gcBlobService.Options.SourceContainer, blobNameFromUri);
        return gcDocument;
    }
}