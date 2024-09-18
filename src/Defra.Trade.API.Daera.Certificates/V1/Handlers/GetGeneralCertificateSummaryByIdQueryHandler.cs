// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Infrastructure;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Repository.Interfaces;
using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using Defra.Trade.API.Daera.Certificates.V1.Queries;
using Defra.Trade.API.Daera.Certificates.V1.Utilities;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

namespace Defra.Trade.API.Daera.Certificates.V1.Handlers;

public class GetGeneralCertificateSummaryByIdQueryHandler(
    IMapper mapper,
    IProtectiveMonitoringService protectiveMonitoringService,
    LinkGenerator linkGenerator,
    IOptions<ApimExternalApisSettings> apiSettings,
    ICertificatesStoreRepository certificatesStoreRepository) : IRequestHandler<GetGeneralCertificateSummaryByIdQuery, GeneralCertificateSummary>
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IProtectiveMonitoringService _protectiveMonitoringService = protectiveMonitoringService ?? throw new ArgumentNullException(nameof(protectiveMonitoringService));
    private readonly LinkGenerator _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    private readonly IOptions<ApimExternalApisSettings> _apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));
    private readonly ICertificatesStoreRepository _certificatesStoreRepository = certificatesStoreRepository ?? throw new ArgumentNullException(nameof(certificatesStoreRepository));

    public async Task<GeneralCertificateSummary> Handle(GetGeneralCertificateSummaryByIdQuery request, CancellationToken cancellationToken)
    {
        var gcSummary = await _certificatesStoreRepository.GetGeneralCertificateSummaryAsync(request.GcId, cancellationToken);

        if (gcSummary == null)
            return null;

        var result = _mapper.Map<GeneralCertificateSummary>(gcSummary);
        var ehcoGc = _mapper.Map<EhcoModels.EhcoGeneralCertificateApplication>(gcSummary);
        _mapper.Map(ehcoGc, result);

        foreach (var itemDocument in result.Documents)
        {
            itemDocument.AddLinkToGetGeneralCertificateDocument(
                _linkGenerator,
                _apiSettings.Value.DaeraCertificatesApiUrlV1,
                result.GeneralCertificateId,
                itemDocument.Id);
        }

        result.AddLinkToGetGeneralCertificate(
            _linkGenerator,
            _apiSettings.Value.DaeraCertificatesApiUrlV1,
            result.GeneralCertificateId);

        await _protectiveMonitoringService.LogSocEventAsync(
            Common.Audit.Enums.TradeApiAuditCode.GeneralCertificateSummaryById,
            "Successfully fetched General Certificate summary by Id");

        return result;
    }
}