// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A particular point in the progression of time together with the relevant supplementary information.
/// </summary>
public class DateTimeType
{
    /// <summary>
    /// The particular point in the progression of time.
    /// </summary>
    /// <example>2023-01-01T12:00:00+0000</example>
    [Required]
    public string Content { get; set; }

    /// <summary>
    /// The format of the date/time content.
    /// </summary>
    /// <example>205</example>
    public string Format { get; set; }
}