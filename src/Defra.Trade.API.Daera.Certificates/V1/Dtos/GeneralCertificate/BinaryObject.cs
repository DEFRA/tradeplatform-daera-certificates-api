// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A binary object that is attached or otherwise appended to this referenced document.
/// </summary>
public class BinaryObject
{
    /// <summary>
    /// The Uniform Resource Identifier that identifies where the Binary Object is located.
    /// </summary>
    public string Uri { get; set; }

    /// <summary>
    /// The filename of the binary object.
    /// </summary>
    public string Filename { get; set; }
}