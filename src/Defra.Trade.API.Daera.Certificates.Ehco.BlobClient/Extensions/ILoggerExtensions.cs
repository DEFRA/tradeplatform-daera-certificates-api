// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Microsoft.Extensions.Logging;

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Extensions;

public static partial class ILoggerExtensions
{
    [LoggerMessage(EventId = 11, EventName = nameof(BlobRetrievalStart), Level = LogLevel.Information, Message = "Retrieving {BlobName} from {Container}")]
    public static partial void BlobRetrievalStart(this ILogger logger, string blobName, string container);

    [LoggerMessage(EventId = 12, EventName = nameof(BlobNotFound), Level = LogLevel.Error, Message = "Blob: {BlobName} from {Container} not found")]
    public static partial void BlobNotFound(this ILogger logger, Exception ex, string blobName, string container);

    [LoggerMessage(EventId = 13, EventName = nameof(BlobRetrievalError), Level = LogLevel.Error, Message = "Error Retrieving file {BlobName} from {Container}")]
    public static partial void BlobRetrievalError(this ILogger logger, Exception ex, string blobName, string container);
}