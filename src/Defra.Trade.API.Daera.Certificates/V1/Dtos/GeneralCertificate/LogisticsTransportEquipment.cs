// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// Logistics transport equipment utilized for this supply chain consignment.
/// </summary>
public class LogisticsTransportEquipment
{
    /// <summary>
    /// The unique identifier of this piece of logistics transport equipment.
    /// </summary>
    public IDType Id { get; set; }

    /// <summary>
    /// A seal affixed to this piece of logistics transport equipment.
    /// </summary>
    public IList<LogisticsSeal> AffixedSeal { get; set; }

    /// <summary>
    /// Temperature settings for a transport movement, such as a required storage temperature range.
    /// </summary>
    public IList<TransportSettingTemperature> TemperatureSetting { get; set; }
}