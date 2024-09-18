// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;
namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A character string denoting the type of a document. 
/// </summary>
public class CodeType
{
    /// <summary>
    /// A character string denoting the type of a document.
    /// </summary>
    /// <example>271</example>
    [Required]
    public string Content { get; set; }

    /// <summary>
    /// An agency that maintains one or more code lists.
    /// </summary>
    /// <example>6</example>
    public string ListAgencyID { get; set; }
}