// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;
using Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;
using MediatR;

namespace Defra.Trade.API.Daera.Certificates.V1.Queries;

/// <summary>
/// Query to handle retrieving a General Certificate by Id.
/// </summary>
public class GetGeneralCertificateByIdQuery : IRequest<GeneralCertificate>
{
    /// <summary>
    /// Identifier of the General Certificate.
    /// </summary>
    [Required]
    public string GcId { get; set; }
}