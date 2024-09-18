// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.V1.Notifications;
using MediatR;

namespace Defra.Trade.API.Daera.Certificates.V1.Handlers;

public sealed class GeneralCertificateDeliveredNotificationHandler : INotificationHandler<GeneralCertificateDeliveredNotification>
{
    private readonly IGeneralCertificateCompletionService _completionService;

    public GeneralCertificateDeliveredNotificationHandler(IGeneralCertificateCompletionService completionService)
    {
        ArgumentNullException.ThrowIfNull(completionService);
        _completionService = completionService;
    }

    public async Task Handle(GeneralCertificateDeliveredNotification notification, CancellationToken cancellationToken)
    {
        await _completionService.GeneralCertificateDelivered(notification.GeneralCertificateId, cancellationToken);
    }
}