// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;
using Defra.Trade.Common.Api.Dtos;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos;

public class ApplicationAttachments : ResourceBase
{
    public string GeneralCertificateId { get; set; }

    public string GeneralCertificateAttachmentId { get; set; }

    public GetBlobResult FileContext { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}