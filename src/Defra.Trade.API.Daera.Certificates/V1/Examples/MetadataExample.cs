// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using Defra.Trade.API.Daera.Certificates.V1.Utilities;
using Swashbuckle.AspNetCore.Filters;

namespace Defra.Trade.API.Daera.Certificates.V1.Examples;

public class MetadataExample : IExamplesProvider<ServiceMetadata>
{
    public ServiceMetadata GetExamples()
    {
        return new ServiceMetadata
        {
            Links =
            [
                new(
                    "https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate-summaries",
                    ResourceLinkConstants.Rel.GeneralCertificateSummaries,
                    HttpMethods.Get,
                    ResourceLinkConstants.Description.GeneralCertificateSummaries),
                new(
                    "https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate-summary?gcId={gcId}",
                    ResourceLinkConstants.Rel.GeneralCertificateSummaryGetById,
                    HttpMethods.Get,
                    ResourceLinkConstants.Description.GeneralCertificateSummaryGetById),
                new(
                    "https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate?gcId={gcId}",
                    ResourceLinkConstants.Rel.GeneralCertificateGetById,
                    HttpMethods.Get,
                    ResourceLinkConstants.Description.GeneralCertificateGetById),
                new(
                    "https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate/document?gcId={gcId}&documentId={documentId}",
                    ResourceLinkConstants.Rel.GeneralCertificateDocumentGetById,
                    HttpMethods.Get,
                    ResourceLinkConstants.Description.GeneralCertificateDocumentGetById)
            ]
        };
    }
}