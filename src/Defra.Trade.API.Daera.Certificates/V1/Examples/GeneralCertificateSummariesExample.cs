// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using Defra.Trade.API.Daera.Certificates.V1.Dtos.Enums;
using Defra.Trade.API.Daera.Certificates.V1.Utilities;
using Defra.Trade.Common.Api.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Defra.Trade.API.Daera.Certificates.V1.Examples;

public class GeneralCertificateSummariesExample : IExamplesProvider<PagedResult<GeneralCertificateSummary>>
{
    public PagedResult<GeneralCertificateSummary> GetExamples()
    {
        var result = new List<GeneralCertificateSummary>
        {
            GetGcExampleSummary("GC-12001"),
            GetGcExampleSummary("GC-12002")
        };
        var pagedResult = new PagedResult<GeneralCertificateSummary>
        {
            Data = result,
            PageNumber = 1,
            PageSize = 10,
            Records = 2,
            TotalRecords = 2
        };
        return pagedResult;
    }

    private static GeneralCertificateSummary GetGcExampleSummary(string gcId)
    {
        return new GeneralCertificateSummary
        {
            CreatedOn = new DateTimeOffset(2022, 12, 12, 12, 12, 12, TimeSpan.Zero),
            Documents = new List<GeneralCertificateDocumentSummary>
            {
                new()
                {
                    Id = $"{gcId}-doc1",
                    TypeCode =  "tc1",
                    Links =
                    [
                        new($"https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate/document?gcId={gcId}&documentId={gcId}-doc1",
                            ResourceLinkConstants.Rel.GeneralCertificateDocumentGetById,
                            HttpMethods.Get,
                            ResourceLinkConstants.Description.GeneralCertificateDocumentGetById)
                    ]
                }
            },
            Status = CertificateStatus.Complete,
            GeneralCertificateId = gcId,
            LastUpdated = new DateTimeOffset(2023, 12, 12, 12, 12, 12, TimeSpan.Zero),
            Links =
                    [
                        new(
                            $"https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate-summary?gcId={gcId}",
                            ResourceLinkConstants.Rel.GeneralCertificateSummaryGetById,
                            HttpMethods.Get,
                            ResourceLinkConstants.Description.GeneralCertificateSummaryGetById),
                        new(
                            $"https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate?gcId={gcId}",
                            ResourceLinkConstants.Rel.GeneralCertificateGetById,
                            HttpMethods.Get,
                            ResourceLinkConstants.Description.GeneralCertificateGetById)
                    ],
        };
    }
}