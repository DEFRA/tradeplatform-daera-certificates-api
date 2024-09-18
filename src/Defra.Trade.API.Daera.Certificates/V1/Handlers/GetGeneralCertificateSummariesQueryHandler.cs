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
using CommonDtos = Defra.Trade.Common.Api.Dtos;
using DBModels = Defra.Trade.API.Daera.Certificates.Database.Models;
using LogicModels = Defra.Trade.API.Daera.Certificates.Logic.Models;

namespace Defra.Trade.API.Daera.Certificates.V1.Handlers;

public class GetGeneralCertificateSummariesQueryHandler(
    IMapper mapper,
    IProtectiveMonitoringService protectiveMonitoringService,
    LinkGenerator linkGenerator,
    IOptions<ApimExternalApisSettings> apiSettings,
    ICertificatesStoreRepository certificatesStoreRepository)
    : IRequestHandler<GetGeneralCertificateSummariesQuery, CommonDtos.PagedResult<GeneralCertificateSummary>>
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private readonly IProtectiveMonitoringService _protectiveMonitoringService = protectiveMonitoringService ??
                                           throw new ArgumentNullException(nameof(protectiveMonitoringService));

    private readonly LinkGenerator _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    private readonly IOptions<ApimExternalApisSettings> _apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));

    private readonly ICertificatesStoreRepository _certificatesStoreRepository = certificatesStoreRepository ??
                                           throw new ArgumentNullException(nameof(certificatesStoreRepository));

    public async Task<CommonDtos.PagedResult<GeneralCertificateSummary>> Handle(
        GetGeneralCertificateSummariesQuery request, CancellationToken cancellationToken)
    {
        var generalCertificateSummariesQuery = _mapper.Map<DBModels.GeneralCertificateSummariesQuery>(request);

        var summaryData = await _certificatesStoreRepository.GetGeneralCertificateSummariesAsync(
            generalCertificateSummariesQuery, cancellationToken);

        var pagedSummaries = _mapper.Map<LogicModels.PagedGeneralCertificateSummary>(summaryData);

        var summaries = _mapper.Map<List<GeneralCertificateSummary>>(pagedSummaries.Data);

        foreach (var summary in summaries)
        {
            summary.AddLinkToGetGeneralCertificate(
                _linkGenerator,
                _apiSettings.Value.DaeraCertificatesApiUrlV1,
                summary.GeneralCertificateId);

            summary.AddLinkToGetGeneralCertificateSummary(
                _linkGenerator,
                _apiSettings.Value.DaeraCertificatesApiUrlV1,
                summary.GeneralCertificateId);
        }

        await _protectiveMonitoringService.LogSocEventAsync(
            Common.Audit.Enums.TradeApiAuditCode.GeneralCertificateSummary,
            "Successfully fetched General Certificate summaries");

        return new CommonDtos.PagedResult<GeneralCertificateSummary>(summaries, request, pagedSummaries.TotalRecords);
    }
}