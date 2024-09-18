// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A character string to identify and distinguish uniquely, one instance of an object in an identification scheme from all other objects in the same scheme together with relevant supplementary information.
/// </summary>
public class IDType
{
    /// <summary>
    /// A character string to identify and distinguish uniquely, one instance of an object in an identification scheme from all other objects within the same scheme.
    /// </summary>
    /// <example>CHEDP.XI.2023.0000001</example>
    [Required]
    public string Content { get; set; }

    /// <summary>
    /// The identification of the agency that maintains the identification scheme.
    /// </summary>
    /// <example>5</example>
    public string SchemeAgencyId { get; set; }

    /// <summary>
    /// The identification of the identification scheme.
    /// </summary>
    /// <example>GC</example>
    public string SchemeId { get; set; }
}