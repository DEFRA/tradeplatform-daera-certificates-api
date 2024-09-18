// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Services;
using Defra.Trade.Common.Audit.Enums;
using Microsoft.Extensions.Logging;
using protectiveMonitoring = Defra.Trade.ProtectiveMonitoring.Interfaces;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Services;

public class ProtectiveMonitoringServiceTests
{
    private readonly Mock<ILogger<ProtectiveMonitoringService>> _loggerMock;
    private readonly Mock<protectiveMonitoring.IProtectiveMonitoringService> _socProtectiveMonitoringServiceMock;
    private readonly ProtectiveMonitoringService _sut;

    public ProtectiveMonitoringServiceTests()
    {
        _loggerMock = new Mock<ILogger<ProtectiveMonitoringService>>();
        _socProtectiveMonitoringServiceMock = new Mock<protectiveMonitoring.IProtectiveMonitoringService>();
        _sut = GetDefaultSut();
    }

    [Fact]
    public async Task LogSocEvent_Ok()
    {
        _socProtectiveMonitoringServiceMock
            .Setup(r => r.LogSocEventAsync(TradeApiAuditCode.GeneralCertificateSummary,
                "Successfully fetched General Certificate summaries - ok", string.Empty))
            .Returns(Task.CompletedTask);

        await _sut.LogSocEventAsync(TradeApiAuditCode.GeneralCertificateSummary,
            "Successfully fetched General Certificate summaries - ok");

        _socProtectiveMonitoringServiceMock.Verify(
            r => r.LogSocEventAsync(TradeApiAuditCode.GeneralCertificateSummary,
                "Successfully fetched General Certificate summaries - ok", string.Empty), Times.Once);
    }

    [Fact]
    public async Task LogSocEvent_Error()
    {
        _socProtectiveMonitoringServiceMock
            .Setup(r => r.LogSocEventAsync(TradeApiAuditCode.GeneralCertificateSummary,
                "Successfully fetched General Certificate summaries - error", string.Empty))
            .Returns(Task.FromException(new Exception()));

        await _sut.LogSocEventAsync(TradeApiAuditCode.GeneralCertificateSummary,
            "Successfully fetched General Certificate summaries - error");

        _socProtectiveMonitoringServiceMock.Verify(
            r => r.LogSocEventAsync(TradeApiAuditCode.GeneralCertificateSummary,
                "Successfully fetched General Certificate summaries - error", string.Empty), Times.Once);
    }

    private ProtectiveMonitoringService GetDefaultSut() => new(
        _loggerMock.Object,
        _socProtectiveMonitoringServiceMock.Object);
}
