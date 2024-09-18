// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Defra.Trade.API.Daera.Certificates.Database.Context;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Defra.Trade.API.Daera.Certificates.Repository;

public class CertificatesStoreRepository(DaeraCertificateDbContext context) : ICertificatesStoreRepository
{
    private readonly DaeraCertificateDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<GeneralCertificate> GetGeneralCertificateWithEnrichmentAsync(string gcId, CancellationToken cancellationToken = default)
    {
        return await _context.GeneralCertificate
            .FindGeneralCertificate(gcId)
            .WhereHasEnrichmentData()
            .IncludeEnrichmentData()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<GeneralCertificate> GetGeneralCertificateSummaryAsync(string gcId,
        CancellationToken cancellationToken = default)
    {
        return await _context.GeneralCertificate
            .FindGeneralCertificate(gcId)
            .WhereHasEnrichmentData()
            .IncludeEnrichmentData()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedGeneralCertificateSummary> GetGeneralCertificateSummariesAsync(GeneralCertificateSummariesQuery query, CancellationToken cancellationToken = default)
    {
        int totalRecordsCount = await _context.GeneralCertificate
            .WhereHasEnrichmentData()
            .WhereMatched(query)
            .CountAsync(cancellationToken);

        var data = await _context.GeneralCertificate
            .WhereHasEnrichmentData()
            .WhereMatched(query)
            .Ordered(query)
            .Paginated(query)
            .SelectSummaries()
            .ToListAsync(cancellationToken);

        return new PagedGeneralCertificateSummary(data, totalRecordsCount);
    }

    /// <inheritdoc />
    public async Task<GeneralCertificate> GetGeneralCertificateWithDocumentsAsync(string gcId, CancellationToken cancellationToken = default)
    {
        return await _context.GeneralCertificate
                   .FindGeneralCertificate(gcId)
                   .WhereHasEnrichmentData()
                   .IncludeDocumentsData()
                   .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<GeneralCertificate> GetGeneralCertificateWithDocumentsAsync(string gcId, string documentId, CancellationToken cancellationToken = default)
    {
        return await _context.GeneralCertificate
                   .FindGeneralCertificate(gcId)
                   .WhereHasEnrichmentData()
                   .IncludeDocumentWithDocIdData(documentId)
                   .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IList<GeneralCertificateDocument>> GetAllGeneralCertificateDocuments(string gcId, CancellationToken cancellationToken = default)
    {
        var result = await _context.GeneralCertificate
            .FindGeneralCertificate(gcId)
            .IncludeDocumentsData()
            .FirstOrDefaultAsync(cancellationToken);

        return result.GeneralCertificateDocument;
    }
}