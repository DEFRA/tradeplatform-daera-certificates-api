// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Net.Http.Json;
using Defra.Trade.API.Daera.Certificates.IntegrationTests.Infrastructure;
using Defra.Trade.API.Daera.Certificates.IntegrationTests.V1.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Defra.Trade.API.Daera.Certificates.IntegrationTests.V1.Controllers.MonitorController;

public class HealthTests(DaeraCertificatesApplicationFactory<Startup> webApplicationFactory) : IClassFixture<DaeraCertificatesApplicationFactory<Startup>>
{
    private readonly string _defaultClientIpAddress = "12.34.56.789";
    private readonly DaeraCertificatesApplicationFactory<Startup> _webApplicationFactory = webApplicationFactory;

    [Fact]
    public async Task Health_WhenCalled_ShouldReturnHealthy()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        _webApplicationFactory.AddApimUserContextHeaders(client, Guid.Empty, _defaultClientIpAddress);

        // Act
        var response = await client.GetAsync("health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var healthReports = await response.Content.ReadFromJsonAsync<TestHealthReportResponse>();

        healthReports.Should().NotBeNull();
        if (healthReports == null)
        {
            Assert.False(true);
        }

        healthReports.Status.Should().Be(HealthStatus.Healthy);
        foreach (var item in healthReports.Entries)
        {
            item.Status.Should().Be(HealthStatus.Healthy);
        }
    }
}
