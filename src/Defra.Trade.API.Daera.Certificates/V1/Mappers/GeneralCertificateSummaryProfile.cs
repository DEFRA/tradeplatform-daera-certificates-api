// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using DBModels = Defra.Trade.API.Daera.Certificates.Database;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using LogicModels = Defra.Trade.API.Daera.Certificates.Logic.Models;

namespace Defra.Trade.API.Daera.Certificates.V1.Mappers;

public class GeneralCertificateSummaryProfile : Profile
{
    public GeneralCertificateSummaryProfile()
    {
        CreateMap<LogicModels.GeneralCertificateSummary, Dtos.GeneralCertificateSummary>()
            .ForMember(d => d.GeneralCertificateId, opt => opt.MapFrom(s => s.GeneralCertificateId))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
            .ForMember(d => d.CreatedOn, opt => opt.MapFrom(s => s.CreatedOn))
            .ForMember(d => d.LastUpdated, opt => opt.MapFrom(s => s.LastUpdated))
            .ForMember(d => d.Documents, opt => opt.MapFrom(s => s.Documents));

        CreateMap<LogicModels.GeneralCertificateDocumentSummary, Dtos.GeneralCertificateDocumentSummary>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.TypeCode, opt => opt.MapFrom(s => s.DocumentTypeCode));

        CreateMap<DBModels.Models.GeneralCertificateSummary, LogicModels.GeneralCertificateSummary>();

        CreateMap<DBModels.Models.GeneralCertificateSummary, Dtos.GeneralCertificateSummary>();

        CreateMap<DBModels.Models.GeneralCertificateDocumentSummary, Dtos.GeneralCertificateDocumentSummary>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.TypeCode, opt => opt.MapFrom(s => s.DocumentTypeCode));

        CreateMap<EhcoModels.EhcoGeneralCertificateApplication, Dtos.GeneralCertificateSummary>()
            .ForMember(d => d.GeneralCertificateId, opt => opt.MapFrom(s => s.ExchangedDocument.ID))
            .ForMember(d => d.Documents, opt => opt.MapFrom(s => GetDocumentSummaries(s.ExchangedDocument)));

        CreateMap<DBModels.Models.GeneralCertificate, Dtos.GeneralCertificateSummary>()
            .ForMember(d => d.GeneralCertificateId, opt => opt.MapFrom(s => s.GeneralCertificateId))
            .ForMember(d => d.CreatedOn, opt => opt.MapFrom(gc => gc.CreatedOn < gc.EnrichmentData.CreatedOn ? gc.CreatedOn : gc.EnrichmentData.CreatedOn))
            .ForMember(d => d.LastUpdated, opt => opt.MapFrom(gc => gc.LastUpdatedOn > gc.EnrichmentData.LastUpdatedOn ? gc.LastUpdatedOn : gc.EnrichmentData.LastUpdatedOn));
    }

    private static List<Dtos.GeneralCertificateDocumentSummary> GetDocumentSummaries(EhcoModels.ExchangedDocument exchangedDocument)
    {
        var packingListSummary = GetDocumentSummary(exchangedDocument.PackingListFileLocation, "271");
        var certificatePdfSummary = GetDocumentSummary(exchangedDocument.CertificatePDFLocation, "16");

        var summaries = new List<Dtos.GeneralCertificateDocumentSummary> { packingListSummary };

        if (certificatePdfSummary.Id is not null)
            summaries.Add(certificatePdfSummary);

        return summaries;
    }

    private static Dtos.GeneralCertificateDocumentSummary GetDocumentSummary(string fileLocation, string documentTypeCode)
    {
        if (!Uri.IsWellFormedUriString(fileLocation, UriKind.Absolute))
            return new Dtos.GeneralCertificateDocumentSummary();

        var uri = new Uri(fileLocation);
        string fileName = uri.Segments.LastOrDefault();
        string fileExtension = System.IO.Path.GetExtension(fileName);

        if (string.IsNullOrEmpty(fileExtension))
            return new Dtos.GeneralCertificateDocumentSummary();

        return new Dtos.GeneralCertificateDocumentSummary
        {
            Id = fileName,
            TypeCode = documentTypeCode
        };
    }
}