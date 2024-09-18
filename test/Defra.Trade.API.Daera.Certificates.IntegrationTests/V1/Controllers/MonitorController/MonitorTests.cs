// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Net.Http.Json;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.IntegrationTests.Infrastructure;
using Defra.Trade.API.Daera.Certificates.IntegrationTests.V1.Models;
using Defra.Trade.API.Daera.Certificates.Logic.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Defra.Trade.API.Daera.Certificates.IntegrationTests.V1.Controllers.MonitorController;

public class MonitorTests(DaeraCertificatesApplicationFactory<Startup> webApplicationFactory)
    : IClassFixture<DaeraCertificatesApplicationFactory<Startup>>
{
    private readonly string _defaultClientIpAddress = "12.34.56.789";

    private readonly DaeraCertificatesApplicationFactory<Startup> _webApplicationFactory = webApplicationFactory
        ?? throw new ArgumentNullException(nameof(webApplicationFactory));

    [Fact]
    public async Task Monitor_WhenCalled_ShouldReturnHealthy()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        _webApplicationFactory.AddApimUserContextHeaders(client, Guid.Empty, _defaultClientIpAddress);

        var monitorResponse = new HealthReportResponse()
        {
            Status = HealthStatus.Healthy,
            Entries =
                [
                    new()
                    {
                        Key = "first",
                        Status = HealthStatus.Healthy,
                        Description = "description",
                        DurationMs = 3,
                        ExceptionMessage = new ArithmeticException().Message,
                        Data = new DatabaseInfo()
                        {
                            CanConnect = true,
                            CurrentMigration = "currentMigration",
                            DatabaseName = "databaseName",
                            PendingMigrations = new List<string>()
                        }
                    }
                ],
            TotalDurationMs = 4
        };

        _webApplicationFactory.MonitorService
            .Setup(x => x.GetReport())
            .ReturnsAsync(monitorResponse);

        // Act
        var response = await client.GetAsync("monitor");

        // Assert
        var content = await response.Content.ReadFromJsonAsync<TestHealthReportResponse>();
        content.Should().NotBeNull();
        content.Status.Should().Be(HealthStatus.Healthy);
        content.Entries.Count.Should().Be(1);
        content.TotalDurationMs.Should().BeGreaterThanOrEqualTo(0);

        var responseEntry = content.Entries[0];
        var sourceEntry = content.Entries[0];

        responseEntry.Key.Should().Be(sourceEntry.Key);
        responseEntry.Status.Should().Be(sourceEntry.Status);
        responseEntry.Description.Should().Be(sourceEntry.Description);
        responseEntry.DurationMs.Should().Be(sourceEntry.DurationMs);
        responseEntry.ExceptionMessage.Should().NotBeNull();
        responseEntry.Data.CurrentMigration.Should().Be(sourceEntry.Data.CurrentMigration);
        responseEntry.Data.CanConnect.Should().Be(sourceEntry.Data.CanConnect);
        responseEntry.Data.DatabaseName.Should().Be(sourceEntry.Data.DatabaseName);
        responseEntry.Data.PendingMigrations.Count().Should().Be(0);
        responseEntry.Tags.Should().BeEmpty();
    }

    [Fact]
    public async Task Monitor_WhenCalled_ShouldReturnUnHealthyWith500()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        _webApplicationFactory.AddApimUserContextHeaders(client, Guid.Empty, _defaultClientIpAddress);

        var monitorResponse = new HealthReportResponse()
        {
            Status = HealthStatus.Unhealthy,
            Entries =
                [
                    new()
                    {
                        Key = "first",
                        Status = HealthStatus.Unhealthy,
                        Description = "description",
                        DurationMs = 3,
                        ExceptionMessage = new ArithmeticException().Message,
                        Data = new DatabaseInfo()
                        {
                            CanConnect = true,
                            CurrentMigration = "currentMigration",
                            DatabaseName = "databaseName",
                            PendingMigrations = new List<string>()
                        }
                    }
                ],
            TotalDurationMs = 4
        };

        _webApplicationFactory.MonitorService
            .Setup(x => x.GetReport())
            .ReturnsAsync(monitorResponse);

        // Act
        var response = await client.GetAsync("monitor");

        // Assert
        var content = await response.Content.ReadFromJsonAsync<TestHealthReportResponse>();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        content.Should().NotBeNull();
        content.Status.Should().Be(HealthStatus.Unhealthy);
        content.Entries.Count.Should().Be(1);
    }

    [Fact]
    public async Task Monitor_WhenCalled_ShouldReturnNullWithProblem()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        _webApplicationFactory.AddApimUserContextHeaders(client, Guid.Empty, _defaultClientIpAddress);

        _webApplicationFactory.MonitorService
            .Setup(x => x.GetReport())
            .ReturnsAsync((HealthReportResponse)null);

        // Act
        var response = await client.GetAsync("monitor");

        // Assert
        var content = await response.Content.ReadFromJsonAsync<TestHealthReportResponse>();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        content.Entries.Should().BeEmpty();
    }
}
