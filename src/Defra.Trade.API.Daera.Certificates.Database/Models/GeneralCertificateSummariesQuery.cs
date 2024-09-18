// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Database.Models.Enum;

namespace Defra.Trade.API.Daera.Certificates.Database.Models;

public class GeneralCertificateSummariesQuery
{
    public long PageNumber { get; set; }
    public long PageSize { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
    public DateTimeOffset? ModifiedSince { get; set; }
}

