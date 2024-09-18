// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;

namespace Defra.Trade.API.Daera.Certificates.Ehco.DocumentProvider;

/// <summary>
/// General certificate document provider.
/// </summary>
public interface IGcDocumentProvider
{
    /// <summary>
    /// Return General certificate document.
    /// </summary>
    /// <param name="blobName"></param>
    /// <returns>blob-content</returns>
    Task<GetBlobResult> GetGcDocumentById(string blobName);
}