// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A note included in this exchanged document.
/// </summary>
public class DocumentClause
{
    /// <summary>
    /// A unique identifier for this note.
    /// </summary>
    [Required]
    public IDType Id { get; set; }

    /// <summary>
    /// A code specifying the content of this note.
    /// </summary>
    [Required]
    public TextType Content { get; set; }
}