// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models;

public class GeneralCertificateAttachment
{
    public string GeneralCertificateId { get; set; }

    public string GeneralCertificateDocumentId { get; set; }

    public string FileContext { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public string DocumentTypeCode { get; set; }
}