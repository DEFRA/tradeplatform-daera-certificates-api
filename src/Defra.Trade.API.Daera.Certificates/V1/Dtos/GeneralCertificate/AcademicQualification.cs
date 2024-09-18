// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// An academic qualification attained by a person.
/// </summary>
public class AcademicQualification
{
    /// <summary>
    /// A name, expressed as text, of this qualification.
    /// </summary>
    public TextType Name { get; set; }

    /// <summary>
    /// The abbreviated name, expressed as text, of this qualification
    /// </summary>
    public TextType AbbreviatedName { get; set; }
}