// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// The details of a authoritative signatory person of a trade party.
/// </summary>
public class AuthoritativeSignatoryPerson
{
    /// <summary>
    /// The name, expressed as text, of this person.
    /// </summary>
    [Required]
    public TextType Name { get; set; }

    /// <summary>
    /// An academic qualification attained by this person.
    /// </summary>
    public IList<AcademicQualification> AttainedAcademicQualification { get; set; }
}