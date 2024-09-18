// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Text.Json.Serialization;

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

public sealed class GcStatusUpdate
{
    [JsonPropertyName("applicationId")]
    public double? ApplicationId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("modifiedon")]
    public DateTimeOffset TimestampUTC { get; set; }
}