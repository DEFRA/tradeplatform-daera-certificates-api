// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A character string (i.e. a finite set of characters) generally in the form of words of a language.
/// </summary>
public class TextType
{
    /// <summary>
    /// A character string (i.e. a finite set of characters) generally in the form of words of a language.
    /// </summary>
    /// <example>Test content example</example>
    [Required]
    public string Content { get; set; }

    /// <summary>
    /// The identifier of the language used in the corresponding text string.
    /// </summary>
    /// <example>EN</example>
    public string LanguageId { get; set; }
}