// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A logistics transport movement specified for this supply chain consignment.
/// </summary>
public class LogisticsTransportMovement
{
    /// <summary>
    /// The code specifying the mode, such as by air, sea, rail, road or inland waterway, for this logistics transport movement.
    /// </summary>
    [Required]
    public CodeType ModeCode { get; set; }

    /// <summary>
    /// The unique identifier for this logistics transport movement, such as a voyage number, flight number, or trip number.
    /// </summary>
    public IDType Id { get; set; }

    /// <summary>
    /// The means of transport used for this logistics transport movement.
    /// </summary>
    [Required]
    public LogisticsTransportMeans UsedTransportMeans { get; set; }
}