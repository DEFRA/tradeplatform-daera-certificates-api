// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoMapper;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class EhcoOperatorProfile : Profile
{
    public EhcoOperatorProfile()
    {
        CreateMap<EhcoModels.Operator, GcModels.TradeParty>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Name, opt => opt.MapFrom(s => new List<GcModels.TextType> { MapTextType(s.Name) }))
            .ForMember(d => d.RoleCode, opt => opt.MapFrom(s => new List<GcModels.CodeType> { new() { Content = "AG" } }))
            .ForMember(d => d.PostalAddress, opt => opt.MapFrom(s => s))
            .ForMember(d => d.EmailAddress, opt => opt.MapFrom(s => MapCommunication(s.Email)))
            .ForMember(d => d.Telephone, opt => opt.MapFrom(s => MapCommunication(s.Telephone)))
            .ForMember(d => d.AuthoritativeSignatoryPerson, opt => opt.Ignore())
            .ForMember(d => d.DefinedContactDetails, opt => opt.Ignore());

        CreateMap<EhcoModels.Operator, GcModels.TradeAddress>()
            .ForMember(d => d.Postcode, opt => opt.MapFrom(s => MapCodeType(s.Postcode)))
            .ForMember(d => d.LineOne, opt => opt.MapFrom(s => MapTextType(s.LineOne)))
            .ForMember(d => d.LineTwo, opt => opt.MapFrom(s => MapTextType(s.LineTwo)))
            .ForMember(d => d.LineThree, opt => opt.Ignore())
            .ForMember(d => d.LineFour, opt => opt.Ignore())
            .ForMember(d => d.LineFive, opt => opt.Ignore())
            .ForMember(d => d.CityName, opt => opt.MapFrom(s => MapTextType(s.CityName)))
            .ForMember(d => d.CountryCode, opt => opt.MapFrom(s => MapId(s.CountryCode, "5")))
            .ForMember(d => d.CountryName, opt => opt.Ignore())
            .ForMember(d => d.CountrySubDivisionCode, opt => opt.Ignore())
            .ForMember(d => d.CountrySubDivisionName, opt => opt.Ignore())
            .ForMember(d => d.TypeCode, opt => opt.MapFrom(s => new GcModels.CodeType { Content = "1", ListAgencyID = "6" }))
            ;
    }

    private static GcModels.TextType MapTextType(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.TextType
        {
            Content = source,
            LanguageId = "en"
        };
    }

    private static GcModels.CodeType MapCodeType(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.CodeType
        {
            Content = source
        };
    }

    private static GcModels.IDType MapId(string source, string schemeAgencyId)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.IDType
        {
            Content = source,
            SchemeAgencyId = schemeAgencyId
        };
    }

    private static GcModels.UniversalCommunication MapCommunication(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.UniversalCommunication
        {
            CompleteNumber = new GcModels.TextType
            {
                Content = source
            }
        };
    }
}
