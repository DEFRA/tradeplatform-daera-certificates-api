// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.Common.Audit.Enums;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;

public interface IProtectiveMonitoringService
{
    Task LogSocEventAsync(TradeApiAuditCode auditCode, string message, string additionalInfo = "");
}