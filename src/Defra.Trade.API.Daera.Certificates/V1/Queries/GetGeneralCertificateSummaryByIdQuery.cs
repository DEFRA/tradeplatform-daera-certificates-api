// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;
using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using MediatR;

namespace Defra.Trade.API.Daera.Certificates.V1.Queries;

/// <summary>
/// Query to handle retrieving a General Certificate summary by Id.
/// </summary>
public class GetGeneralCertificateSummaryByIdQuery : IRequest<GeneralCertificateSummary>
{
    /// <summary>
    /// Identifier of the General Certificate.
    /// </summary>
    [Required]
    public string GcId { get; set; }
}
