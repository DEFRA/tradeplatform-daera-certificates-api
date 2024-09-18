// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using Defra.Trade.API.Daera.Certificates.V1.Dtos.Enums;
using MediatR;
using CommonDtos = Defra.Trade.Common.Api.Dtos;

namespace Defra.Trade.API.Daera.Certificates.V1.Queries;

/// <summary>
/// Query to handle retrieving a list of General Certificate summaries.
/// </summary>
public class GetGeneralCertificateSummariesQuery : CommonDtos.PagedQuery, IRequest<CommonDtos.PagedResult<GeneralCertificateSummary>>
{
    public GetGeneralCertificateSummariesQuery()
    {
        PageNumber = 1;
        PageSize = 1000;
    }

    /// <summary>
    /// Last Modified Date sort order. Default is Asc.
    /// </summary>
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;

    /// <summary>
    /// Last modified date time value.
    /// </summary>
    public DateTimeOffset? ModifiedSince { get; set; }
}
