// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Diagnostics;
using Defra.Trade.API.Daera.Certificates.Database.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;
using Defra.Trade.API.Daera.Certificates.Logic.Models;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services;

/// <inheritdoc />
/// <summary>
/// Initializes and new instance of <see cref="MonitorService"/>
/// </summary>
/// <param name="dbHealthCheckService">The database health check service</param>
/// <param name="storageAccountHealthCheckService">The storage account health check service</param>
/// <exception cref="ArgumentNullException"></exception>
public class MonitorService(
    IDbHealthCheckService dbHealthCheckService,
    IAzureBlobService<GcDocumentBlobStorageOptions> storageAccountHealthCheckService) : IMonitorService
{
    private readonly IDbHealthCheckService _dbHealthCheckService = dbHealthCheckService ?? throw new ArgumentNullException(nameof(dbHealthCheckService));
    private readonly IAzureBlobService<GcDocumentBlobStorageOptions> _storageAccountHealthCheckService = storageAccountHealthCheckService ?? throw new ArgumentNullException(nameof(storageAccountHealthCheckService));

    /// <inheritdoc />
    public async Task<HealthReportResponse> GetReport()
    {
        var sw = Stopwatch.StartNew();

        HealthReportResponse healthReportResponse = new();
        var dbEntry = new HealthCheckResultEntry("DaeraCertificateDbContext");

        try
        {
            var storageAccountHealthResponse = await _storageAccountHealthCheckService.CheckStorageAccountHealth();

            var saEntry = new HealthCheckResultEntry
            {
                Status = storageAccountHealthResponse.Status,
                Key = nameof(GcDocumentBlobStorageOptions),
                Description = storageAccountHealthResponse.Description
            };

            var databaseInfo = await _dbHealthCheckService.GetContextInfo();

            var healthStatus = databaseInfo.PendingMigrations.Any() ? HealthStatus.Unhealthy : HealthStatus.Healthy;

            dbEntry.Data = databaseInfo;
            dbEntry.DurationMs = (int)sw.ElapsedMilliseconds;
            dbEntry.Status = healthStatus;

            healthReportResponse.Entries.Add(dbEntry);
            healthReportResponse.Entries.Add(saEntry);
            healthReportResponse.TotalDurationMs = (int)sw.ElapsedMilliseconds;
            healthReportResponse.Status = healthReportResponse.Entries
                .Exists(x => x.Status == HealthStatus.Unhealthy) ?
                HealthStatus.Unhealthy :
                HealthStatus.Healthy;
        }
        catch (SqlException ex)
        {
            dbEntry.ExceptionMessage = $"SqlException: {ex.Message}";
            dbEntry.Status = HealthStatus.Unhealthy;
            dbEntry.DurationMs = (int)sw.ElapsedMilliseconds;

            healthReportResponse.Status = HealthStatus.Unhealthy;
            healthReportResponse.TotalDurationMs = (int)sw.ElapsedMilliseconds;
            healthReportResponse.Entries.Add(dbEntry);
        }

        return healthReportResponse;
    }
}