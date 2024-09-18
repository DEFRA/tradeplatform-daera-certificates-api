// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Database.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;
using Defra.Trade.API.Daera.Certificates.Logic.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Services;

public class MonitorServiceTests
{
    private readonly Mock<IDbHealthCheckService> _dbHealthCheckServiceMock;
    private readonly Mock<IAzureBlobService<GcDocumentBlobStorageOptions>> _storageAccountServiceMock;
    private readonly MonitorService _sut;

    public MonitorServiceTests()
    {
        _dbHealthCheckServiceMock = new();
        _storageAccountServiceMock = new();
        _sut = new MonitorService(
            _dbHealthCheckServiceMock.Object,
            _storageAccountServiceMock.Object);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void Ctor_NullArg_Throws(bool p1IsNull, bool p2IsNull)
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new MonitorService(
                p1IsNull ? null : _dbHealthCheckServiceMock.Object,
                p2IsNull ? null : _storageAccountServiceMock.Object));
    }

    [Fact]
    public async Task GetReport_ZeroPendingMigrations_ReturnsHealthy()
    {
        // Arrange
        var sourceDbInfo = new DatabaseInfo()
        {
            CanConnect = true,
            CurrentMigration = "currentMigration",
            DatabaseName = "databaseName",
            PendingMigrations = new List<string>()
        };

        var sourceSaInfo = new HealthCheckResult(
            HealthStatus.Healthy,
            "storageAccountNamespace",
            null,
            null);

        _dbHealthCheckServiceMock.Setup(x => x
            .GetContextInfo())
            .ReturnsAsync(sourceDbInfo);

        _storageAccountServiceMock.Setup(x => x
            .CheckStorageAccountHealth(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceSaInfo);

        // Act
        var result = await _sut.GetReport();

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Healthy);
        result.TotalDurationMs.Should().BeGreaterThanOrEqualTo(0);
        result.Entries.Should().HaveCount(2);

        var dbResultEntry = result.Entries[0];
        dbResultEntry.Description.Should().BeNull();
        dbResultEntry.DurationMs.Should().BeGreaterThanOrEqualTo(0);
        dbResultEntry.ExceptionMessage.Should().BeNull();
        dbResultEntry.Key.Should().Be("DaeraCertificateDbContext");
        dbResultEntry.Status.Should().Be(HealthStatus.Healthy);
        dbResultEntry.Tags.Should().BeEmpty();
        dbResultEntry.Data.CanConnect.Should().BeTrue();
        dbResultEntry.Data.CurrentMigration.Should().Be(sourceDbInfo.CurrentMigration);
        dbResultEntry.Data.DatabaseName.Should().Be(sourceDbInfo.DatabaseName);
        dbResultEntry.Data.PendingMigrations.Should().BeEmpty();

        var saResultEntry = result.Entries[1];
        saResultEntry.Status.Should().Be(HealthStatus.Healthy);
        saResultEntry.DurationMs.Should().BeGreaterThanOrEqualTo(0);
        saResultEntry.Description.Should().Be("storageAccountNamespace");
    }

    [Fact]
    public async Task GetReport_WithPendingMigrations_ReturnsUnhealthy()
    {
        // Arrange
        var sourceDbInfo = new DatabaseInfo()
        {
            CanConnect = true,
            CurrentMigration = "currentMigration",
            DatabaseName = "databaseName",
            PendingMigrations = new List<string>()
            {
                "pendingMigration0",
                "pendingMigration1"
            }
        };

        _dbHealthCheckServiceMock.Setup(x => x
            .GetContextInfo())
            .ReturnsAsync(sourceDbInfo);

        // Act
        var result = await _sut.GetReport();

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Unhealthy);
        result.TotalDurationMs.Should().BeGreaterThanOrEqualTo(0);
        result.Entries.Should().HaveCount(2);

        var resultEntry = result.Entries[0];
        resultEntry.Description.Should().BeNull();
        resultEntry.DurationMs.Should().BeGreaterThanOrEqualTo(0);
        resultEntry.ExceptionMessage.Should().BeNull();
        resultEntry.Key.Should().Be("DaeraCertificateDbContext");
        resultEntry.Status.Should().Be(HealthStatus.Unhealthy);
        resultEntry.Tags.Should().BeEmpty();

        resultEntry.Data.CanConnect.Should().BeTrue();
        resultEntry.Data.CurrentMigration.Should().Be(sourceDbInfo.CurrentMigration);
        resultEntry.Data.DatabaseName.Should().Be(sourceDbInfo.DatabaseName);
        resultEntry.Data.PendingMigrations.Should().HaveCount(2);
        resultEntry.Data.PendingMigrations.ToList()[0].Should().Be(sourceDbInfo.PendingMigrations.ToList()[0]);
        resultEntry.Data.PendingMigrations.ToList()[1].Should().Be(sourceDbInfo.PendingMigrations.ToList()[1]);
    }

    [Fact]
    public async Task GetReport_WithSqlException_ReturnsUnhealthyWithExceptionMessage()
    {
        // Arrange
        _dbHealthCheckServiceMock.Setup(x => x
            .GetContextInfo())
            .ThrowsAsync(CreateSqlException());

        // Act
        var result = await _sut.GetReport();

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HealthStatus.Unhealthy);
        result.TotalDurationMs.Should().BeGreaterThanOrEqualTo(0);
        result.Entries.Should().HaveCount(1);

        var resultEntry = result.Entries[0];
        resultEntry.Description.Should().BeNull();
        resultEntry.DurationMs.Should().BeGreaterThanOrEqualTo(0);
        resultEntry.ExceptionMessage.Should().StartWith("SqlException:");
        resultEntry.Key.Should().Be("DaeraCertificateDbContext");
        resultEntry.Status.Should().Be(HealthStatus.Unhealthy);
        resultEntry.Tags.Should().BeEmpty();
        resultEntry.Data.Should().BeNull();
    }

    private static SqlException CreateSqlException()
    {
        SqlException exception = null;
        try
        {
            var conn = new SqlConnection(@"Data Source=.;Database=GUARANTEED_TO_FAIL;Connection Timeout=1");
            conn.Open();
        }
        catch (SqlException ex)
        {
            exception = ex;
        }

        return exception;
    }
}
