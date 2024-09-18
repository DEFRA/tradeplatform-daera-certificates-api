// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// The means of transport used for this logistics transport movement.
/// </summary>
public class LogisticsTransportMeans
{
    /// <summary>
    /// An identifier of this logistics means of transport, such as the International Maritime Organization number of a vessel.
    /// </summary>
    [Required]
    public IDType Id { get; set; }

    /// <summary>
    /// The name, expressed as text, of this logistics means of transport.
    /// </summary>
    public TextType Name { get; set; }
    /// <summary>
    /// The code specifying the type of logistics transport means
    /// </summary>
    [Required]
    public CodeType TypeCode { get; set; }
}