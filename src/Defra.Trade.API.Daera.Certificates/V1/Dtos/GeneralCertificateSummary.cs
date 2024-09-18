// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Dtos.Enums;
using Defra.Trade.Common.Api.Dtos;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos;

/// <summary>
/// Summary information regarding a General Certificate.
/// </summary>
public class GeneralCertificateSummary : ResourceBase
{
    /// <summary>
    /// Identifier of the General Certificate.
    /// </summary>
    public string GeneralCertificateId { get; set; }

    /// <summary>
    /// Status of the General Certificate.
    /// </summary>
    public CertificateStatus Status { get; set; }

    /// <summary>
    /// Documents for the General Certificate.
    /// </summary>
    public IReadOnlyCollection<GeneralCertificateDocumentSummary> Documents { get; set; } = new List<GeneralCertificateDocumentSummary>();

    /// <summary>
    /// Date/time when the General Certificate was created.
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// Date/time when the General Certificate was last updated.
    /// </summary>
    public DateTimeOffset LastUpdated { get; set; }
}