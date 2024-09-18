// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

/// <summary>
/// The means of transport used for this logistics transport movement.
/// </summary>
public class LogisticsTransportMeans
{
    /// <summary>
    /// An identifier of this logistics means of transport, such as the International Maritime Organization number of a vessel.
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// The code specifying the mode for this logistics transport movement.
    /// </summary>
    public string ModeCode { get; set; }
}