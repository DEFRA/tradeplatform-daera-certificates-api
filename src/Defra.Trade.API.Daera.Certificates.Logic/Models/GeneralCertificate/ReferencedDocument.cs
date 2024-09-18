// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

public class ReferencedDocument
{
    public CodeType TypeCode { get; set; }

    public IList<IDType> Id { get; set; }

    public IList<BinaryObject> AttachedBinaryObject { get; set; }
}