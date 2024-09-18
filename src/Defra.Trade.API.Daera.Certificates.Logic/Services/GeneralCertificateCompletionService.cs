// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.CertificatesStore.V1.ApiClient.Api;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services;

public class GeneralCertificateCompletionService : IGeneralCertificateCompletionService
{
    private static readonly Action<ILogger, string, Exception> _allDocsNotRetrieved = LoggerMessage.Define<string>(LogLevel.Information, default, "All documents not yet retrieved by DAERA for {GeneralCertificateId}, notification to EHCO deferred.");
    private static readonly int _gcCertPdfTypeCode = 16;
    private static readonly int _packingListTypeCode = 271;
    private readonly IDocumentRetrievalApi _documentRetrievalApi;
    private readonly IEhcoNotificationService _ehcoNotifier;
    private readonly ICertificatesStoreRepository _generalCertificatesRepository;
    private readonly ILogger<GeneralCertificateCompletionService> _logger;

    public GeneralCertificateCompletionService(
        IEhcoNotificationService ehcoNotifier,
        ICertificatesStoreRepository generalCertificatesRepository,
        IDocumentRetrievalApi documentRetrievalApi,
        ILogger<GeneralCertificateCompletionService> logger)
    {
        ArgumentNullException.ThrowIfNull(ehcoNotifier);
        ArgumentNullException.ThrowIfNull(generalCertificatesRepository);
        ArgumentNullException.ThrowIfNull(logger);

        _ehcoNotifier = ehcoNotifier;
        _generalCertificatesRepository = generalCertificatesRepository;
        _documentRetrievalApi = documentRetrievalApi;
        _logger = logger;
    }

    public async Task DocumentDelivered(string gcId, string documentId, CancellationToken cancellationToken)
    {
        var gc = await _generalCertificatesRepository.GetGeneralCertificateWithDocumentsAsync(gcId, cancellationToken);

        var docIds = GetDocumentIds(Schema2Docs, gc.GeneralCertificateDocument);
        bool allDocsRetrieved = await GetRetrievalStatus(docIds);

        if (!allDocsRetrieved)
        {
            _allDocsNotRetrieved(_logger, gcId, null);
            return;
        }

        await _ehcoNotifier.NotifyGeneralCertificateDelivered(gcId, cancellationToken);
    }

    public Task GeneralCertificateDelivered(string gcId, CancellationToken cancellationToken)
    {
        // This isnt needed at the moment, but in the future it might be used to check that all parts of the GC have been requested.
        return Task.CompletedTask;
    }

    private static List<Guid> GetDocumentIds(
        Func<GeneralCertificateDocument, bool> predicate,
        IEnumerable<GeneralCertificateDocument> documents)
    {
        return documents
            .Where(predicate)
            .Select(d => d.Id)
            .ToList();
    }

    private static bool Schema2Docs(GeneralCertificateDocument d)
    {
        return d.TypeCode == _packingListTypeCode || d.TypeCode == _gcCertPdfTypeCode;
    }

    private async Task<bool> GetRetrievalStatus(List<Guid> docIds)
    {
        foreach (var docId in docIds)
        {
            bool docHasBeenRetrieved = await _documentRetrievalApi.GetDocumentRetrievedAsync(docId);
            if (!docHasBeenRetrieved)
            {
                return false;
            }
        }

        return true;
    }
}