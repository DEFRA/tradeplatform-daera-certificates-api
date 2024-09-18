// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Repository.Interfaces;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services;

public class GeneralCertificateAggregatorService : IGeneralCertificateAggregatorService
{
    private readonly ICertificatesStoreRepository _certificatesStoreRepository;
    private readonly IModelMapper<Database.Models.GeneralCertificate, GcModels.GeneralCertificate> _generalCertificateEnrichmentMapper;

    public GeneralCertificateAggregatorService(
        ICertificatesStoreRepository certificatesStoreRepository,
        IModelMapper<Database.Models.GeneralCertificate, GcModels.GeneralCertificate> generalCertificateEnrichmentMapper)
    {
        ArgumentNullException.ThrowIfNull(certificatesStoreRepository);
        ArgumentNullException.ThrowIfNull(generalCertificateEnrichmentMapper);
        _certificatesStoreRepository = certificatesStoreRepository;
        _generalCertificateEnrichmentMapper = generalCertificateEnrichmentMapper;
    }

    public async Task<GcModels.GeneralCertificate> GetEnrichedGeneralCertificateAsync(string gcId, CancellationToken cancellationToken = default)
    {
        var dbGc = await _certificatesStoreRepository.GetGeneralCertificateWithEnrichmentAsync(gcId, cancellationToken);
        if (dbGc?.EnrichmentData.Data == null)
        {
            return null;
        }

        var gcResultPayload = _generalCertificateEnrichmentMapper.Map(dbGc);
        return gcResultPayload;
    }
}