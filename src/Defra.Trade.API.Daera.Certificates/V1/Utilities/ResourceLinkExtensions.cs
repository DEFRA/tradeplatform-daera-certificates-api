// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Controllers;
using Defra.Trade.Common.Api.Dtos;
using Microsoft.AspNetCore.Routing;

namespace Defra.Trade.API.Daera.Certificates.V1.Utilities;

public static class ResourceLinkExtensions
{
    public static ResourceBase AddLinkToGetGeneralCertificatesSummaries(this ResourceBase resource, LinkGenerator linkGenerator, string basePath)
    {
        string href = basePath
                   + linkGenerator.GetPathByAction(
                       nameof(GeneralCertificatesSummaryController.GetGeneralCertificateSummaries),
                       nameof(GeneralCertificatesSummaryController).Replace("Controller", string.Empty));

        resource.Links ??= [];
        resource.Links.Add(new ResourceLink
        {
            Href = href,
            Method = HttpMethods.Get,
            Rel = ResourceLinkConstants.Rel.GeneralCertificateSummaries,
            Description = ResourceLinkConstants.Description.GeneralCertificateSummaries
        });

        return resource;
    }

    public static ResourceBase AddLinkToGetGeneralCertificateSummary(this ResourceBase resource, LinkGenerator linkGenerator, string basePath, string gcId)
    {
        string href = basePath
                   + linkGenerator.GetPathByAction(
                           nameof(GeneralCertificatesSummaryController.GetGeneralCertificateSummaryById),
                           nameof(GeneralCertificatesSummaryController).Replace("Controller", string.Empty),
                           new { gcId = gcId ?? "C925B478-311E-4596-B11C-D659E9C3B576" })
                       ?.Replace("C925B478-311E-4596-B11C-D659E9C3B576", "{gcId}");

        resource.Links ??= [];
        resource.Links.Add(new ResourceLink
        {
            Href = href,
            Method = HttpMethods.Get,
            Rel = ResourceLinkConstants.Rel.GeneralCertificateSummaryGetById,
            Description = ResourceLinkConstants.Description.GeneralCertificateSummaryGetById
        });

        return resource;
    }

    public static ResourceBase AddLinkToGetGeneralCertificate(this ResourceBase resource, LinkGenerator linkGenerator, string basePath, string gcId)
    {
        string href = basePath
                   + linkGenerator.GetPathByAction(
                           nameof(GeneralCertificatesController.GetGeneralCertificateById),
                           nameof(GeneralCertificatesController).Replace("Controller", string.Empty),
                           new { gcId = gcId ?? "C925B478-311E-4596-B11C-D659E9C3B576" })
                       ?.Replace("C925B478-311E-4596-B11C-D659E9C3B576", "{gcId}");

        resource.Links ??= [];
        resource.Links.Add(new ResourceLink
        {
            Href = href,
            Method = HttpMethods.Get,
            Rel = ResourceLinkConstants.Rel.GeneralCertificateGetById,
            Description = ResourceLinkConstants.Description.GeneralCertificateGetById
        });

        return resource;
    }

    public static ResourceBase AddLinkToGetGeneralCertificateDocument(this ResourceBase resource, LinkGenerator linkGenerator, string basePath, string gcId, string documentId)
    {
        string href = basePath
                   + linkGenerator.GetPathByAction(
                           nameof(GeneralCertificateDocumentsController.GetGeneralCertificateDocumentById),
                           nameof(GeneralCertificateDocumentsController).Replace("Controller", string.Empty),
                           new { gcId = gcId ?? "C925B478-311E-4596-B11C-D659E9C3B576", documentId = documentId ?? "87e196d8-8d8b-47cf-b39f-5a4c1187cb03" })
                       ?.Replace("C925B478-311E-4596-B11C-D659E9C3B576", "{gcId}")
                       .Replace("87e196d8-8d8b-47cf-b39f-5a4c1187cb03", "{documentId}");

        resource.Links ??= [];
        resource.Links.Add(new ResourceLink
        {
            Href = href,
            Method = HttpMethods.Get,
            Rel = ResourceLinkConstants.Rel.GeneralCertificateDocumentGetById,
            Description = ResourceLinkConstants.Description.GeneralCertificateDocumentGetById
        });

        return resource;
    }
}