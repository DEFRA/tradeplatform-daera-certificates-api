// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Net.Mime;
using Defra.Trade.API.Daera.Certificates.Extensions;
using Defra.Trade.API.Daera.Certificates.Logic.Extensions;
using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using Defra.Trade.API.Daera.Certificates.V1.Examples;
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
/// General Certificates Summary Controller endpoints.
/// </summary>
[ApiVersion("1")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(ApimPassThroughSchemeOptions.Names.DaeraUserPolicy)]
public class GeneralCertificatesSummaryController(
    IMediator mediator,
    IValidator<GetGeneralCertificateSummariesQuery> summariesValidator,
    ILogger<GeneralCertificatesSummaryController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly IValidator<GetGeneralCertificateSummariesQuery> _summariesValidator = summariesValidator ?? throw new ArgumentNullException(nameof(summariesValidator));
    private readonly ILogger<GeneralCertificatesSummaryController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Get a filtered and paginated summary listing of General Certificates.
    /// </summary>
    /// <remarks>
    /// Retrieve summary information of General Certificates with filtering and pagination.
    /// </remarks>
    /// <param name="query">Filter parameters to restrict the summary items returned.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Successfully retrieved the listing of the General Certificate summary details.</response>
    /// <response code="400">The parameters specified were invalid. Please correct before trying again.</response>
    /// <returns>Summary information about the General Certificates.</returns>
    [HttpGet("general-certificate-summaries", Name = "GetGeneralCertificateSummaries")]
    [ProducesResponseType(typeof(CommonDtos.PagedResult<GeneralCertificateSummary>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GeneralCertificateSummariesExample))]
    [ProducesResponseType(typeof(CommonDtos.CommonProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CommonProblemDetailsBadRequestExample))]
    [Audit(LogType = AuditLogType.DaeraCertificatesV1GeneralCertificateSummaryGet, WithResponseBody = true)]
    public async Task<IActionResult> GetGeneralCertificateSummaries([FromQuery] GetGeneralCertificateSummariesQuery query,
        CancellationToken cancellationToken)
    {
        _logger.GeneralCertificateSummariesRequested();

        var validation = await _summariesValidator.ValidateAsync(query, cancellationToken);

        if (!validation.IsValid)
            return this.FluentValidationProblem(validation);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get the summary of a General Certificate.
    /// </summary>
    /// <remarks>
    /// Retrieve summary information for a General Certificate.
    /// </remarks>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Successfully retrieved the General Certificate summary.</response>
    /// <response code="404">The General Certificate summary could not be found.</response>
    /// <returns>Summary information about a General Certificate.</returns>
    [HttpGet("general-certificate-summary", Name = "GetGeneralCertificateSummaryById")]
    [ProducesResponseType(typeof(GeneralCertificateSummary), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GeneralCertificateSummaryExample))]
    [ProducesResponseType(typeof(CommonDtos.CommonProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(CommonProblemDetailsNotFoundExample))]
    [Audit(LogType = AuditLogType.DaeraCertificatesV1GeneralCertificateSummaryGetById, SystemRequestIdQueryName = "gcId", WithResponseBody = true)]
    public async Task<IActionResult> GetGeneralCertificateSummaryById([FromQuery] GetGeneralCertificateSummaryByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.GeneralCertificateSummaryRequested(query.GcId);

        var result = await _mediator.Send(query, cancellationToken);

        return result == null
            ? NotFound()
            : Ok(result);
    }
}