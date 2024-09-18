// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Text.Json.Serialization;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Defra.Trade.API.Daera.Certificates.Logic.Models;

public sealed class HealthReportResponse
{
    [JsonPropertyName("status")]
    public HealthStatus Status { get; set; }

    [JsonPropertyName("entries")]
    public List<HealthCheckResultEntry> Entries { get; set; } = [];

    [JsonPropertyName("totalDurationMs")]
    public int TotalDurationMs { get; set; }
}

public sealed class HealthCheckResultEntry
{
    public string Key { get; set; }
    public DatabaseInfo Data { get; set; }
    public string Description { get; set; }
    public string ExceptionMessage { get; set; }
    public int DurationMs { get; set; }
    public HealthStatus Status { get; set; }
    public List<string> Tags { get; set; } = [];

    public HealthCheckResultEntry()
    {
    }

    public HealthCheckResultEntry(string key)
    {
        Key = key;
    }
}