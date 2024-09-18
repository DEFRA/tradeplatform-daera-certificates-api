// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.Common.Api.Dtos;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos;

/// <summary>
/// General Certificate Document summary.
/// </summary>
public class GeneralCertificateDocumentSummary : ResourceBase
{
    /// <summary>
    /// Document id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// type code.
    /// </summary>
    public string TypeCode { get; set; }
}