// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Net.Mime;
using Defra.Trade.API.Daera.Certificates.Logic.Infrastructure;
using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using Defra.Trade.API.Daera.Certificates.V1.Examples;
using Defra.Trade.API.Daera.Certificates.V1.Utilities;
using Defra.Trade.Common.ExternalApi.ApimIdentity;
using Defra.Trade.Common.ExternalApi.Auditing;
using Defra.Trade.Common.ExternalApi.Auditing.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;

namespace Defra.Trade.API.Daera.Certificates.V1.Controllers;

/// <summary>
/// Metadata about the Daera Certificates Service.
/// </summary>
[ApiVersion("1")]
[ApiController]
[Route("metadata")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(ApimPassThroughSchemeOptions.Names.DaeraUserPolicy)]
public class MetadataController(LinkGenerator linkGenerator, IOptions<ApimExternalApisSettings> apiSettings) : ControllerBase
{
    private readonly LinkGenerator _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    private readonly IOptions<ApimExternalApisSettings> _apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));

    /// <summary>
    /// Your starting point to discover the endpoints available from the DAERA Retail Movement Scheme Certificates API and how they can be used.
    /// </summary>
    /// <remarks>
    /// This endpoint is provided to help with the discovery and usability of the DAERA Retail Movement Scheme Certificates API.
    ///
    /// In the response, you will find a directory of the key API endpoints that are available with descriptions, links and guidance.
    /// </remarks>
    /// <response code="200">Successfully retrieved metadata about the API.</response>
    /// <returns>Metadata about the API.</returns>
    [HttpGet(Name = "GetMetadata")]
    [ProducesResponseType(typeof(ServiceMetadata), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MetadataExample))]
    [Audit(LogType = AuditLogType.DaeraCertificatesV1Metadata)]
    public IActionResult Get()
    {
        var result = new ServiceMetadata()
            .AddLinkToGetGeneralCertificatesSummaries(_linkGenerator, _apiSettings.Value.DaeraCertificatesApiUrlV1)
            .AddLinkToGetGeneralCertificateSummary(_linkGenerator, _apiSettings.Value.DaeraCertificatesApiUrlV1, null)
            .AddLinkToGetGeneralCertificate(_linkGenerator, _apiSettings.Value.DaeraCertificatesApiUrlV1, null)
            .AddLinkToGetGeneralCertificateDocument(_linkGenerator, _apiSettings.Value.DaeraCertificatesApiUrlV1, null, null);

        return Ok(result);
    }
}