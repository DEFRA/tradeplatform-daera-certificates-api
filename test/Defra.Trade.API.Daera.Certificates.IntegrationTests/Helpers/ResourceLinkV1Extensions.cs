// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.Common.Api.Dtos;

namespace Defra.Trade.API.Daera.Certificates.IntegrationTests.Helpers;

public static class ResourceLinkV1Extensions
{
    public static bool IsLinkToGeneralCertificatesSummaries(this ResourceLink link)
    {
        return link.Rel == "general-certificate-summaries"
               && link.Method == "GET"
               && link.Href.Equals("https://integrationtest-gateway.trade.azure.defra.cloud/daera-certificates/v1/general-certificate-summaries")
               && link.Description.Equals("Endpoint to get a filtered and paginated list of General Certificate summaries.");
    }

    public static bool IsLinkToGetGeneralCertificateSummaryById(this ResourceLink link, string gcId)
    {
        return link.Rel == "general-certificate-summary-getbyid"
               && link.Method == "GET"
               && link.Href.Equals($"https://integrationtest-gateway.trade.azure.defra.cloud/daera-certificates/v1/general-certificate-summary?gcId={gcId}")
               && link.Description.Equals(
                   "Endpoint to get a summary of a General Certificate by Id.");
    }

    public static bool IsLinkToGetGeneralCertificateById(this ResourceLink link, string gcId)
    {
        return link.Rel == "general-certificate-getbyid"
               && link.Method == "GET"
               && link.Href.Equals($"https://integrationtest-gateway.trade.azure.defra.cloud/daera-certificates/v1/general-certificate?gcId={gcId}")
               && link.Description.Equals(
                   "Endpoint to get the detailed payload of a General Certificate by Id.");
    }

    public static bool IsLinkToGetGeneralCertificateDocument(this ResourceLink link, string gcId, string documentId)
    {
        return link.Rel == "general-certificate-document-getbyid"
               && link.Method == "GET"
               && link.Href.Equals($"https://integrationtest-gateway.trade.azure.defra.cloud/daera-certificates/v1/general-certificate/document?gcId={gcId}&documentId={documentId}")
               && link.Description.Equals(
                   "Endpoint to get a document from a General Certificate by GC Id and document Id.");
    }
}
