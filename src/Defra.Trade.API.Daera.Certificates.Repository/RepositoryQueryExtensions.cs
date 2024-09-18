// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Linq;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Database.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace Defra.Trade.API.Daera.Certificates.Repository;

public static class RepositoryQueryExtensions
{
    public static IQueryable<GeneralCertificate> WhereMatched(this IQueryable<GeneralCertificate> source, GeneralCertificateSummariesQuery query)
    {
        return query.ModifiedSince.HasValue
            ? source.Where(x =>
                query.ModifiedSince.Value.ToUniversalTime() < x.LastUpdatedOn ||
                query.ModifiedSince.Value.ToUniversalTime() < x.EnrichmentData.LastUpdatedOn)
            : source;
    }

    public static IQueryable<GeneralCertificate> Ordered(this IQueryable<GeneralCertificate> source,
        GeneralCertificateSummariesQuery query)
    {
        return query.SortOrder == SortOrder.Desc
            ? source.OrderByDescending(gc => gc.LastUpdatedOn)
            : source.OrderBy(gc => gc.LastUpdatedOn);
    }

    public static IQueryable<GeneralCertificate> Paginated(this IQueryable<GeneralCertificate> source,
        GeneralCertificateSummariesQuery query)
    {
        return source
            .Skip((int)((query.PageNumber - 1) * query.PageSize))
            .Take((int)query.PageSize);
    }

    public static IQueryable<GeneralCertificate> WhereHasEnrichmentData(this IQueryable<GeneralCertificate> source)
    {
        return source.Where(e => e.EnrichmentData != null);
    }

    public static IQueryable<GeneralCertificate> IncludeEnrichmentData(this IQueryable<GeneralCertificate> source)
    {
        return source.Include(c => c.EnrichmentData);
    }

    public static IQueryable<GeneralCertificate> FindGeneralCertificate(this IQueryable<GeneralCertificate> source, string gcId)
    {
        return source.Where(c => c.GeneralCertificateId == gcId);
    }

    public static IQueryable<GeneralCertificateSummary> SelectSummaries(this IQueryable<GeneralCertificate> source)
    {
        return source.Select(gc => new GeneralCertificateSummary
        {
            GeneralCertificateId = gc.GeneralCertificateId,
            CreatedOn = gc.CreatedOn < gc.EnrichmentData.CreatedOn ? gc.CreatedOn : gc.EnrichmentData.CreatedOn,
            LastUpdated = gc.LastUpdatedOn > gc.EnrichmentData.LastUpdatedOn ? gc.LastUpdatedOn : gc.EnrichmentData.LastUpdatedOn,
        });
    }

    public static IQueryable<GeneralCertificate> IncludeDocumentsData(this IQueryable<GeneralCertificate> source)
    {
        return source.Include(c => c.GeneralCertificateDocument);
    }

    public static IQueryable<GeneralCertificate> IncludeDocumentWithDocIdData(this IQueryable<GeneralCertificate> source, string documentId)
    {
        return source.Include(c => c.GeneralCertificateDocument
            .Where(x => x.DocumentId == documentId));
    }
}