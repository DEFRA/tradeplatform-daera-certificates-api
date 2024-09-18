// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Defra.Trade.API.Daera.Certificates.Database.Models;

namespace Defra.Trade.API.Daera.Certificates.Repository.Interfaces;

public interface ICertificatesStoreRepository
{
    Task<GeneralCertificate> GetGeneralCertificateSummaryAsync(string gcId, CancellationToken cancellationToken = default);

    Task<GeneralCertificate> GetGeneralCertificateWithEnrichmentAsync(string gcId, CancellationToken cancellationToken = default);

    Task<PagedGeneralCertificateSummary> GetGeneralCertificateSummariesAsync(GeneralCertificateSummariesQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GC with documents.
    /// </summary>
    /// <param name="gcId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GeneralCertificate> GetGeneralCertificateWithDocumentsAsync(string gcId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GC and document filtrated by gcId and by documentId.
    /// </summary>
    /// <param name="gcId"></param>
    /// <param name="documentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GeneralCertificate> GetGeneralCertificateWithDocumentsAsync(string gcId, string documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GC documents for a given GC by gcId
    /// </summary>
    /// <param name="gcId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IList<GeneralCertificateDocument>> GetAllGeneralCertificateDocuments(string gcId, CancellationToken cancellationToken = default);
}