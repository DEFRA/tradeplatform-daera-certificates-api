// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;

public interface IGeneralCertificateCompletionService
{
    Task DocumentDelivered(string gcId, string documentId, CancellationToken cancellationToken);

    Task GeneralCertificateDelivered(string gcId, CancellationToken cancellationToken);
}