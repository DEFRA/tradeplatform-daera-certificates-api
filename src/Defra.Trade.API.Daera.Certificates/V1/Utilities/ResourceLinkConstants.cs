// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.V1.Utilities;

public static class ResourceLinkConstants
{
    public static class Rel
    {
        public const string GeneralCertificateSummaries = "general-certificate-summaries";
        public const string GeneralCertificateSummaryGetById = "general-certificate-summary-getbyid";
        public const string GeneralCertificateGetById = "general-certificate-getbyid";
        public const string GeneralCertificateDocumentGetById = "general-certificate-document-getbyid";
    }

    public static class Description
    {
        public const string GeneralCertificateSummaries = "Endpoint to get a filtered and paginated list of General Certificate summaries.";
        public const string GeneralCertificateSummaryGetById = "Endpoint to get a summary of a General Certificate by Id.";
        public const string GeneralCertificateGetById = "Endpoint to get the detailed payload of a General Certificate by Id.";
        public const string GeneralCertificateDocumentGetById = "Endpoint to get a document from a General Certificate by GC Id and document Id.";
    }
}