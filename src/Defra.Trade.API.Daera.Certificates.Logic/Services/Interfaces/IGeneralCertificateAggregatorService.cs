// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;

public interface IGeneralCertificateAggregatorService
{
    Task<GeneralCertificate> GetEnrichedGeneralCertificateAsync(string gcId,
        CancellationToken cancellationToken = default);
}