// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A seal affixed to this piece of logistics transport equipment.
/// </summary>
public class LogisticsSeal
{
    /// <summary>
    /// A unique identifier for this logistics seal.
    /// </summary>
    [Required]
    public IDType Id { get; set; }
}