// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

/// <summary>
/// Logistics transport equipment utilized for this supply chain consignment.
/// </summary>
public class LogisticsTransportEquipment
{
    /// <summary>
    /// A seal affixed to this piece of logistics transport equipment.
    /// </summary>
    public string AffixedSeal { get; set; }

    /// <summary>
    /// Temperature settings for a transport movement, such as a required storage temperature range.
    /// </summary>
    public string TemperatureSetting { get; set; }

    /// <summary>
    /// The trailer number used in the transport
    /// </summary>
    public string TrailerNumber { get; set; }
}