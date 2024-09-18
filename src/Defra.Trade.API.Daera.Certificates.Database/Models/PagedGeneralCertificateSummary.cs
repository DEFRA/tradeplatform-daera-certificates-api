// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Generic;

namespace Defra.Trade.API.Daera.Certificates.Database.Models;

public class PagedGeneralCertificateSummary(IEnumerable<GeneralCertificateSummary> data, long totalRecords)
{
    public long TotalRecords { get; set; } = totalRecords;

    public IEnumerable<GeneralCertificateSummary> Data { get; set; } = data;
}