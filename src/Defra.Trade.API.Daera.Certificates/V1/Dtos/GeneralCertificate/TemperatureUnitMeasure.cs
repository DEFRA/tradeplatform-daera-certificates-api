// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// The numeric value determined by temperature measuring.
/// </summary>
public class TemperatureUnitMeasure
{
    /// <summary>
    /// The numeric value determined by temperature measuring.
    /// </summary>
    /// <example>1</example>
    [Required]
    public decimal Content { get; set; }
}