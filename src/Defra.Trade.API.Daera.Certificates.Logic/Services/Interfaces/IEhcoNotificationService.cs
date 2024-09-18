// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;

public interface IEhcoNotificationService
{
    Task NotifyGeneralCertificateDelivered(string generalCertificateId, CancellationToken cancellationToken);
}