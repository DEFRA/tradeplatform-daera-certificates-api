// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Text.Json.Serialization;

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

public sealed class EhcoGcUpdateMessage
{
    [JsonPropertyName("messageId")]
    public double? MessageId { get; set; }

    [JsonPropertyName("messageType")]
    public string MessageType { get; set; }

    [JsonPropertyName("applicationUpdate")]
    public GcStatusUpdate ApplicationUpdate { get; set; }
}