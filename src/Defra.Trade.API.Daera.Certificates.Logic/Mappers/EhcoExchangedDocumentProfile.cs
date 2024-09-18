// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoMapper;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class EhcoExchangedDocumentProfile : Profile
{
    public EhcoExchangedDocumentProfile()
    {
        CreateMap<EhcoModels.ExchangedDocument, GcModels.ExchangedDocument>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => MapId(s.ID, "GC")))
            .ForMember(d => d.IssueDateTime, opt => opt.MapFrom(s => MapIssueDateTime(s.CertificateIssueDateTime)))
            .ForMember(d => d.Issuer, opt => opt.MapFrom(s => MapIssuer()))
            .ForMember(d => d.PrimarySignatoryAuthentication, opt => opt.MapFrom(s => MapDocumentAuthentication(s)))
            .ForMember(d => d.ReferenceDocument, opt => opt.MapFrom(s => MapReferenceDocuments(s)))
            .ForMember(d => d.TraderAssignedId, opt => opt.MapFrom(s => MapId(s.ApplicantAssignedID, null)))
            .ForMember(d => d.TypeCode, opt => opt.MapFrom(s => MapTypeCode(s.TypeCode)));
    }

    private static GcModels.IDType MapId(string source, string schemeId)
    {
        if (source is null)
            return null;

        return new GcModels.IDType
        {
            Content = source,
            SchemeId = schemeId
        };
    }

    private static GcModels.CodeType MapTypeCode(string source)
    {
        if (source is null)
            return null;

        return new GcModels.CodeType
        {
            Content = source
        };
    }

    private static GcModels.DateTimeType MapIssueDateTime(DateTimeOffset source)
    {
        return new GcModels.DateTimeType
        {
            Content = source.ToString("yyyyMMddHHmmzzz"),
            Format = "205"
        };
    }

    private static GcModels.DocumentAuthentication MapDocumentAuthentication(EhcoModels.ExchangedDocument exchangedDocument)
    {
        var result = new GcModels.DocumentAuthentication
        {
            IncludedDocumentClause =
            [
                new()
                {
                    Content = new GcModels.TextType
                    {
                        Content = "I, the undersigned operator responsible for the consignment detailed above, certify that to the best of my knowledge and belief the statements made in Part I of this document are true and complete, and I agree to comply with the requirements of Regulation (EU) 2017/625 on official controls, including payment for official controls, as well as for re-dispatching consignments, quarantine or isolation of animals, or costs of euthanasia and disposal where necessary.",
                        LanguageId = "en"
                    },
                    Id = new GcModels.IDType
                    {
                        Content = "GC Declaration Clause 873",
                        SchemeId = "DC"
                    }
                }
            ]
        };

        if (exchangedDocument.ApplicationSubmissionDateTime != default)
        {
            result.ActualDateTime = new GcModels.DateTimeType
            {
                Content = exchangedDocument.ApplicationSubmissionDateTime.ToString("yyyyMMddHHmmzzz"),
                Format = "205"
            };
        }

        if (exchangedDocument.Applicant?.DefraCustomer is not null)
        {
            result.ProvidingTradeParty = new GcModels.TradeParty
            {
                Id =
                [
                    new()
                    {
                        Content = exchangedDocument.Applicant.DefraCustomer.OrgId == Guid.Empty ? string.Empty : exchangedDocument.Applicant.DefraCustomer.OrgId.ToString()
                    }
                ],
                RoleCode =
                [
                    new()
                    {
                        Content = "DGP"
                    }
                ]
            };
        }

        return result;
    }

    private static GcModels.TradeParty MapIssuer()
    {
        return new GcModels.TradeParty
        {
            Name =
            [
                new()
                {
                    Content = "Department for Environment Food & Rural Affairs"
                }
            ]
        };
    }

    private static List<GcModels.ReferencedDocument> MapReferenceDocuments(EhcoModels.ExchangedDocument source)
    {
        var packingListSource = MapReferenceDocument(source.PackingListFileLocation, "271");
        var certificatePdfSource = MapReferenceDocument(source.CertificatePDFLocation, "16");

        var summaries = new List<GcModels.ReferencedDocument> { packingListSource };

        if (certificatePdfSource.Id is not null)
            summaries.Add(certificatePdfSource);

        return summaries;
    }

    private static GcModels.ReferencedDocument MapReferenceDocument(string source, string typeCode)
    {
        if (source is null || !Uri.TryCreate(source, UriKind.Absolute, out var uri))
            return new GcModels.ReferencedDocument();

        return new GcModels.ReferencedDocument
        {
            Id =
            [
                new()
                {
                    Content = uri.Segments.LastOrDefault()
                }
            ],
            TypeCode = new GcModels.CodeType
            {
                Content = typeCode,
                ListAgencyID = "6"
            },
            AttachedBinaryObject =
            [
                new()
                {
                    Uri = uri.ToString(),
                    Filename = uri.Segments.LastOrDefault()
                }
            ]
        };
    }
}