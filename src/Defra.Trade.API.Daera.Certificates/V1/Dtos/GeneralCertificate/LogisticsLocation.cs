// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// The logistics location details for this supply chain consignment 
/// </summary>
public class LogisticsLocation
{
    /// <summary>
    /// A unique identifier for this logistics related location, such as a United Nations Location Code (UNLOCODE)
    /// or GS1 Global Location Number (GLN).
    /// </summary>
    public IList<IDType> Id { get; set; }

    /// <summary>
    /// A name, expressed as text, of this logistics related location.
    /// </summary>
    [Required]
    public TextType Name { get; set; }

    /// <summary>
    /// The postal trade address information for this logistics related location.
    /// </summary>
    public TradeAddress LocationAddress { get; set; }
}