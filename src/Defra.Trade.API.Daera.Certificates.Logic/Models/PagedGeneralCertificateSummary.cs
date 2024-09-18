// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models;

public class PagedGeneralCertificateSummary
{
    public long TotalRecords { get; set; }

    public IEnumerable<GeneralCertificateSummary> Data { get; init; }
}