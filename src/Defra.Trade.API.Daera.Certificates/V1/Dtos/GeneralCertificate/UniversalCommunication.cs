// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// Details of the communication type.
/// </summary>
public class UniversalCommunication
{
    /// <summary>
    /// The Uniform Resource Identifier for this communication type.
    /// </summary>
    public IDType Uri { get; set; }

    /// <summary>
    /// The complete number of this communication type.
    /// </summary>
    public TextType CompleteNumber { get; set; }
}