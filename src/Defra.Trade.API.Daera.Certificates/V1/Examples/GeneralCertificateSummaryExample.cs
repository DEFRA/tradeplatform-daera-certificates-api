// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using Defra.Trade.API.Daera.Certificates.V1.Dtos.Enums;
using Defra.Trade.API.Daera.Certificates.V1.Utilities;
using Swashbuckle.AspNetCore.Filters;

namespace Defra.Trade.API.Daera.Certificates.V1.Examples;

public class GeneralCertificateSummaryExample : IExamplesProvider<GeneralCertificateSummary>
{
    public GeneralCertificateSummary GetExamples()
    {
        return new GeneralCertificateSummary
        {
            CreatedOn = new DateTimeOffset(2022, 12, 12, 12, 12, 12, TimeSpan.Zero),
            Documents = new List<GeneralCertificateDocumentSummary>
            {
                new()
                {
                    Id = "GC-12001-doc1",
                    TypeCode =  "tc1",
                    Links =
                    [
                        new("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate/document?gcId=GC-12001&documentId=GC-12001-doc1",
                            ResourceLinkConstants.Rel.GeneralCertificateDocumentGetById,
                            HttpMethods.Get,
                            ResourceLinkConstants.Description.GeneralCertificateDocumentGetById)
                    ]
                }
            },
            Status = CertificateStatus.Complete,
            GeneralCertificateId = "GC-12001",
            LastUpdated = new DateTimeOffset(2023, 12, 12, 12, 12, 12, TimeSpan.Zero),
            Links =
            [
                new(
                    "https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate?gcId=GC-12001",
                    ResourceLinkConstants.Rel.GeneralCertificateGetById,
                    HttpMethods.Get,
                    ResourceLinkConstants.Description.GeneralCertificateGetById)
            ]
        };
    }
}