// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.ProtectiveMonitoring.Mappers;
using AC = Defra.Trade.Common.Audit.Enums.TradeApiAuditCode;
using PMC = Defra.Trade.ProtectiveMonitoring.Models.ProtectiveMonitoringCode;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class ProtectiveMonitoringCodeMapper : ProtectiveMonitoringMapperBase
{
    public ProtectiveMonitoringCodeMapper()
    {
        Map(AC.GeneralCertificateSummary, PMC.BusinessTransactions);
        Map(AC.GeneralCertificateSummaryById, PMC.BusinessTransactions);
        Map(AC.GeneralCertificateById, PMC.BusinessTransactions);
        Map(AC.GeneralCertificateDocumentById, PMC.BusinessTransactions);
    }
}