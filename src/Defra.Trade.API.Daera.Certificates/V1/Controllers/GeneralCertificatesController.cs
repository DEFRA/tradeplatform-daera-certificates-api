// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Net.Mime;
using Defra.Trade.API.Daera.Certificates.Extensions;
using Defra.Trade.API.Daera.Certificates.Logic.Extensions;
using Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;
using Defra.Trade.API.Daera.Certificates.V1.Examples;
using Defra.Trade.API.Daera.Certificates.V1.Notifications;
using Defra.Trade.API.Daera.Certificates.V1.Queries;
using Defra.Trade.Common.Api.OpenApi;
using Defra.Trade.Common.ExternalApi.ApimIdentity;
using Defra.Trade.Common.ExternalApi.Auditing;
using Defra.Trade.Common.ExternalApi.Auditing.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Filters;

namespace Defra.Trade.API.Daera.Certificates.V1.Controllers;

/// <summary>
/// General Certificates endpoints.
/// </summary>
[ApiVersion("1")]
[ApiController]
[Route("general-certificate")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(ApimPassThroughSchemeOptions.Names.DaeraUserPolicy)]
public class GeneralCertificatesController(
    IMediator mediator,
    ILogger<GeneralCertificatesController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly ILogger<GeneralCertificatesController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Get details of a General Certificate.
    /// </summary>
    /// <remarks>
    /// Retrieve details for a General Certificate.
    /// </remarks>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Successfully retrieved the General Certificate.</response>
    /// <response code="404">The General Certificate could not be found.</response>
    /// <returns>Details about a General Certificate.</returns>
    [HttpGet(Name = "GetGeneralCertificateById")]
    [ProducesResponseType(typeof(GeneralCertificate), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GeneralCertificateExample))]
    [ProducesResponseType(typeof(Common.Api.Dtos.CommonProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(CommonProblemDetailsNotFoundExample))]
    [Audit(LogType = AuditLogType.DaeraCertificatesV1GeneralCertificateGetById, SystemRequestIdQueryName = "gcId", WithResponseBody = true)]
    public async Task<IActionResult> GetGeneralCertificateById([FromQuery] GetGeneralCertificateByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.GeneralCertificateRequestedRequested(query.GcId);

        var result = await _mediator.Send(query, cancellationToken);
        if (result is null)
            return NotFound();

        Response.OnCompletedSuccessfully(() => _mediator.Publish(new GeneralCertificateDeliveredNotification(query.GcId)));

        return Ok(result);
    }
}