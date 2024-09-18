// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// Other documents referenced by this exchanged document.
/// </summary>
public class ReferencedDocument
{
    /// <summary>
    /// The code specifying the type of referenced document.
    /// </summary>
    public CodeType TypeCode { get; set; }

    /// <summary>
    /// The unique issuer assigned identifier for this referenced document.
    /// </summary>
    public IList<IDType> Id { get; set; }

    /// <summary>
    /// A binary object that is attached or otherwise appended to this referenced document.
    /// </summary>
    public IList<BinaryObject> AttachedBinaryObject { get; set; }
}