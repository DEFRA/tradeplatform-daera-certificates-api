// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Models.Enums;

namespace Defra.Trade.API.Daera.Certificates.Logic.Models;

public class GeneralCertificateSummary
{
    public string GeneralCertificateId { get; set; }

    public SummaryStatus Status { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public IReadOnlyCollection<GeneralCertificateDocumentSummary> Documents { get; set; } = new List<GeneralCertificateDocumentSummary>();
}