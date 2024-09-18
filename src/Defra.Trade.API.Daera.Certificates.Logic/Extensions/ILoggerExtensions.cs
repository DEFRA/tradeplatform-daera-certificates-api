// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Microsoft.Extensions.Logging;

namespace Defra.Trade.API.Daera.Certificates.Logic.Extensions;

public static partial class ILoggerExtensions
{
    [LoggerMessage(EventId = 0, EventName = nameof(LogStartup), Level = LogLevel.Information, Message = "Starting {EnvironmentName} {ApplicationName} from {ContentRootPath}")]
    public static partial void LogStartup(this ILogger logger, string environmentName, string applicationName, string contentRootPath);

    [LoggerMessage(EventId = 5, EventName = nameof(SocEventFailure), Level = LogLevel.Critical, Message = "Failed to send audit event for ProtectiveMonitoring")]
    public static partial void SocEventFailure(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 10, EventName = nameof(MissingBlob), Level = LogLevel.Error, Message = "Missing blob for {GcId}")]
    public static partial void MissingBlob(this ILogger logger, string gcId);

    [LoggerMessage(EventId = 15, EventName = nameof(DocumentRequested), Level = LogLevel.Information, Message = "Document {DocumentId} requested for {GcId}")]
    public static partial void DocumentRequested(this ILogger logger, string gcId, string documentId);

    [LoggerMessage(EventId = 20, EventName = nameof(GeneralCertificateRequestedRequested), Level = LogLevel.Information, Message = "General certificate {GcId} requested")]
    public static partial void GeneralCertificateRequestedRequested(this ILogger logger, string gcId);

    [LoggerMessage(EventId = 25, EventName = nameof(GeneralCertificateSummariesRequested), Level = LogLevel.Information, Message = "List of general certificate summaries requested.")]
    public static partial void GeneralCertificateSummariesRequested(this ILogger logger);

    [LoggerMessage(EventId = 26, EventName = nameof(GeneralCertificateSummaryRequested), Level = LogLevel.Information, Message = "General certificate {GcId} summary requested.")]
    public static partial void GeneralCertificateSummaryRequested(this ILogger logger, string gcId);
}