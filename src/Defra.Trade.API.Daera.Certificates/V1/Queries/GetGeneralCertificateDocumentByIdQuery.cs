// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;
using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using MediatR;

namespace Defra.Trade.API.Daera.Certificates.V1.Queries;

/// <summary>
/// Query to handle retrieving a Document related to a General Certificate by Id.
/// </summary>
public class GetGeneralCertificateDocumentByIdQuery : IRequest<ApplicationAttachments>
{
    /// <summary>
    /// Identifier of the General Certificate.
    /// </summary>
    [Required]
    public string GcId { get; set; }

    /// <summary>
    /// Identifier of the Document related to the General Certificate.
    /// </summary>
    [Required]
    public string DocumentId { get; set; }
}