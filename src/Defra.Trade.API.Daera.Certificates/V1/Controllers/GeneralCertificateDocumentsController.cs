// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.IO;
using System.Net.Mime;
using Defra.Trade.API.Daera.Certificates.Extensions;
using Defra.Trade.API.Daera.Certificates.Logic.Extensions;
using Defra.Trade.API.Daera.Certificates.V1.Notifications;
using Defra.Trade.API.Daera.Certificates.V1.Queries;
using Defra.Trade.Common.Api.OpenApi;
using Defra.Trade.Common.ExternalApi.ApimIdentity;
using Defra.Trade.Common.ExternalApi.Auditing;
using Defra.Trade.Common.ExternalApi.Auditing.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Filters;
using CommonDtos = Defra.Trade.Common.Api.Dtos;

namespace Defra.Trade.API.Daera.Certificates.V1.Controllers;

/// <summary>
/// General Certificate Documents endpoints.
/// </summary>
[ApiVersion("1")]
[ApiController]
[Route("general-certificate/document")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(ApimPassThroughSchemeOptions.Names.DaeraUserPolicy)]
public class GeneralCertificateDocumentsController(
    IMediator mediator,
    ILogger<GeneralCertificateDocumentsController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly ILogger<GeneralCertificateDocumentsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Get a document from a General Certificate by GC id and document id.
    /// </summary>
    /// <remarks>
    /// Returns a document from a General Certificate.
    /// </remarks>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Successfully retrieved a document from the General Certificate.</response>
    /// <response code="400">The parameters specified were invalid. Please correct before trying again.</response>
    /// <returns>Document from a General Certificate.</returns>
    [HttpGet(Name = "GetGeneralCertificateDocumentById")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CommonDtos.CommonProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CommonProblemDetailsBadRequestExample))]
    [ProducesResponseType(typeof(CommonDtos.CommonProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(CommonProblemDetailsNotFoundExample))]
    [Audit(LogType = AuditLogType.DaeraCertificatesV1GeneralCertificateDocumentGetById, SystemRequestIdQueryName = "gcId")]
    public async Task<IActionResult> GetGeneralCertificateDocumentById(
        [FromQuery] GetGeneralCertificateDocumentByIdQuery query,
        CancellationToken cancellationToken)
    {
        _logger.DocumentRequested(query.GcId, query.DocumentId);

        var attachment = await _mediator.Send(query, cancellationToken);

        if (attachment is not { FileContext: { ContentType: string contentType, FileContent: Stream content } })
        {
            return NotFound();
        }

        Response.OnCompletedSuccessfully(() => _mediator.Publish(new GeneralCertificateDocumentDeliveredNotification(query.GcId, query.DocumentId)));

        return File(content, contentType);
    }
}