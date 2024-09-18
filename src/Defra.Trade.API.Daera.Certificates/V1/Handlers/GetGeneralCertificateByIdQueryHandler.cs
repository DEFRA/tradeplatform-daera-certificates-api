// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;
using Defra.Trade.API.Daera.Certificates.V1.Queries;
using MediatR;

namespace Defra.Trade.API.Daera.Certificates.V1.Handlers;

public class GetGeneralCertificateByIdQueryHandler(
    IMapper mapper,
    IGeneralCertificateAggregatorService aggregatorService,
    IProtectiveMonitoringService protectiveMonitoringService) : IRequestHandler<GetGeneralCertificateByIdQuery, GeneralCertificate>
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IGeneralCertificateAggregatorService _aggregatorService = aggregatorService ?? throw new ArgumentNullException(nameof(aggregatorService));
    private readonly IProtectiveMonitoringService _protectiveMonitoringService = protectiveMonitoringService ?? throw new ArgumentNullException(nameof(protectiveMonitoringService));

    public async Task<GeneralCertificate> Handle(GetGeneralCertificateByIdQuery request, CancellationToken cancellationToken)
    {
        var gcPayload = await _aggregatorService.GetEnrichedGeneralCertificateAsync(request.GcId, cancellationToken);

        var result = _mapper.Map<GeneralCertificate>(gcPayload);

        if (result != null)
        {
            await _protectiveMonitoringService.LogSocEventAsync(Common.Audit.Enums.TradeApiAuditCode.GeneralCertificateById,
                "Successfully fetched General Certificate by Id");
        }

        return result;
    }
}