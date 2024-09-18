// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.IO;
using Defra.Trade.API.CertificatesStore.V1.ApiClient.Api;
using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;
using Defra.Trade.API.Daera.Certificates.Ehco.DocumentProvider;
using Defra.Trade.API.Daera.Certificates.Logic.Extensions;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Repository.Interfaces;
using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using Defra.Trade.API.Daera.Certificates.V1.Queries;
using MediatR;

namespace Defra.Trade.API.Daera.Certificates.V1.Handlers;

public class GetGeneralCertificateDocumentByIdQueryHandler(
    ICertificatesStoreRepository certificatesStoreRepository,
    IProtectiveMonitoringService protectiveMonitoringService,
    IGcDocumentProvider gcDocumentProvider,
    IDocumentRetrievalApi documentRetrievalApi,
    ILogger<GetGeneralCertificateDocumentByIdQueryHandler> logger) : IRequestHandler<GetGeneralCertificateDocumentByIdQuery, ApplicationAttachments>
{
    private readonly ICertificatesStoreRepository _certificatesStoreRepository = certificatesStoreRepository ?? throw new ArgumentNullException(nameof(certificatesStoreRepository));
    private readonly IDocumentRetrievalApi _documentRetrievalApi = documentRetrievalApi ?? throw new ArgumentNullException(nameof(documentRetrievalApi));
    private readonly IGcDocumentProvider _gcDocumentProvider = gcDocumentProvider ?? throw new ArgumentNullException(nameof(gcDocumentProvider));
    private readonly ILogger<GetGeneralCertificateDocumentByIdQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IProtectiveMonitoringService _protectiveMonitoringService = protectiveMonitoringService ?? throw new ArgumentNullException(nameof(protectiveMonitoringService));

    public async Task<ApplicationAttachments> Handle(GetGeneralCertificateDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var gcSummary = await _certificatesStoreRepository.GetGeneralCertificateWithDocumentsAsync(request.GcId, request.DocumentId, cancellationToken);

        if (gcSummary == null)
        {
            return null;
        }

        var gcDocument = gcSummary.GeneralCertificateDocument.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(gcDocument?.Url))
        {
            return null;
        }

        GetBlobResult gcDocumentContent;
        try
        {
            gcDocumentContent = await _gcDocumentProvider.GetGcDocumentById(gcDocument.Url);
            await _documentRetrievalApi.SaveDocumentRetrievedAsync(gcDocument.Id, cancellationToken: cancellationToken);
        }
        catch (FileNotFoundException)
        {
            _logger.MissingBlob(request.GcId);
            return null;
        }

        var result = new ApplicationAttachments
        {
            CreatedOn = gcDocument.CreatedOn,
            LastUpdated = gcDocument.LastUpdatedOn,
            GeneralCertificateAttachmentId = gcDocument.DocumentId,
            GeneralCertificateId = gcSummary.GeneralCertificateId,
            FileContext = gcDocumentContent
        };

        await _protectiveMonitoringService.LogSocEventAsync(
            Common.Audit.Enums.TradeApiAuditCode.GeneralCertificateDocumentById,
            "Successfully fetched General Certificate document by Id");

        return result;
    }
}