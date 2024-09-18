// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;

public class GetBlobResult
{
    /// <summary>
    /// Blob content.
    /// </summary>
    public Stream? FileContent { get; set; }

    /// <summary>
    /// Content type.
    /// </summary>
    public string? ContentType { get; set; }
}