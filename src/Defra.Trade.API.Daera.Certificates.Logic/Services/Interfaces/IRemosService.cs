// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Models;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;

public interface IRemosService
{
    Task<GeneralCertificateAttachment> GetGeneralCertificateAttachmentByIdAsync(
     string gcId,
     string documentId,
     CancellationToken cancellationToken = default);
}