// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoMapper;
using Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services;

public sealed class EhcoNotificationService : IEhcoNotificationService
{
    private const string DeliveredToDaera = "DeliveredToDAERA";
    private const string GcUpdatedByTrade = "GC_TP_APPLICATION_UPDATE";
    private const string PublisherId = "TradeAPI";
    private const string StatusNotification = "trade.remos.status.notification";

    private static readonly Action<ILogger, string, Exception> _cannotReadSubmissionId = LoggerMessage.Define<string>(LogLevel.Error, default, "Failed to parse the submission id from general certificate {GeneralCertificateId}.");
    private static readonly Action<ILogger, long, string, Exception> _sendingGcDeliveredNotification = LoggerMessage.Define<long, string>(LogLevel.Information, default, "Sending message {MessageId} to EHCO to notify the successful delivery of General Certificate {GeneralCertificateId} to DAERA.");
    private static readonly Action<ILogger, long, string, Exception> _sentGcDeliveredNotification = LoggerMessage.Define<long, string>(LogLevel.Information, default, "Sent message {MessageId} to EHCO to notify the successful delivery of General Certificate {GeneralCertificateId} to DAERA.");

    private readonly ICertificatesStoreRepository _generalCertificates;
    private readonly IServiceBusClientFactory _serviceBusFactory;
    private readonly ILogger<EhcoNotificationService> _logger;
    private readonly IMapper _mapper;
    private readonly IOptions<EhcoNotificationOptions> _options;

    public EhcoNotificationService(
        IServiceBusClientFactory serviceBusFactory,
        ICertificatesStoreRepository generalCertificates,
        IMapper mapper,
        ILogger<EhcoNotificationService> logger,
        IOptions<EhcoNotificationOptions> options)
    {
        ArgumentNullException.ThrowIfNull(serviceBusFactory);
        ArgumentNullException.ThrowIfNull(generalCertificates);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);

        _serviceBusFactory = serviceBusFactory;
        _generalCertificates = generalCertificates;
        _mapper = mapper;
        _logger = logger;
        _options = options;
    }

    public async Task NotifyGeneralCertificateDelivered(string generalCertificateId, CancellationToken cancellationToken)
    {
        var gcRaw = await _generalCertificates.GetGeneralCertificateSummaryAsync(generalCertificateId, cancellationToken);

        var gc = _mapper.Map<EhcoGeneralCertificateApplication>(gcRaw);
        if (gc is not { ExchangedDocument.ApplicationSubmissionID: string s } || !long.TryParse(s, out long submissionId))
        {
            _cannotReadSubmissionId(_logger, generalCertificateId, null);
            return;
        }

        var opt = _options.Value;
        var serviceBus = _serviceBusFactory.Create(opt.ConnectionString);
        await using var queue = serviceBus.CreateSender(opt.QueueName);
        var timestamp = DateTimeOffset.UtcNow;
        long messageId = Random.Shared.NextInt64(int.MaxValue);

        _sendingGcDeliveredNotification(_logger, messageId, generalCertificateId, null);

        await queue.SendMessageAsync(new()
        {
            ContentType = "application/json",
            SessionId = submissionId.ToString(),
            Subject = StatusNotification,
            ApplicationProperties =
            {
                ["EntityKey"] = submissionId,
                ["PublisherId"] = PublisherId,
                ["SchemaVersion"] = "1",
                ["TimestampUtc"] = timestamp.ToUnixTimeSeconds(),
            },
            Body = new BinaryData(JsonSerializer.Serialize(new EhcoGcUpdateMessage
            {
                MessageId = messageId,
                MessageType = GcUpdatedByTrade,
                ApplicationUpdate = new()
                {
                    ApplicationId = submissionId,
                    Status = DeliveredToDaera,
                    TimestampUTC = timestamp
                }
            }))
        }, cancellationToken);

        _sentGcDeliveredNotification(_logger, messageId, generalCertificateId, null);
    }
}