// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoMapper;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;
using IdcomsModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class IdcomsGeneralCertificateEnrichmentProfile : Profile
{
    public IdcomsGeneralCertificateEnrichmentProfile()
    {
        CreateMap<IdcomsModels.IdcomsGeneralCertificateEnrichment, GcModels.GeneralCertificate>()
            .ForMember(d => d.ExchangedDocument, opt => opt.Ignore())
            .ForMember(d => d.SupplyChainConsignment, opt => opt.MapFrom(s => s));

        CreateMap<IdcomsModels.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>()
            .ForMember(d => d.Consignee, opt => opt.MapFrom((s, d) => MapConsign(s.Organisations, d.Consignee)))
            .ForMember(d => d.Consignor, opt => opt.MapFrom((s, d) => MapConsign(s.Organisations, d.Consignor)))
            .ForMember(d => d.DispatchLocation, opt => opt.MapFrom((s, d) => MapLocation(s.Establishments, d.DispatchLocation)))
            .ForMember(d => d.DestinationLocation, opt => opt.MapFrom((s, d) => MapLocation(s.Establishments, d.DestinationLocation)))
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CustomsId, opt => opt.Ignore())
            .ForMember(d => d.ExportExitDateTime, opt => opt.Ignore())
            .ForMember(d => d.OperatorResponsibleForConsignment, opt => opt.Ignore())
            .ForMember(d => d.BorderControlPostLocation, opt => opt.Ignore())
            .ForMember(d => d.UsedTransportEquipment, opt => opt.Ignore())
            .ForMember(d => d.MainCarriageTransportMovement, opt => opt.Ignore())
            .ForMember(d => d.OriginCountry, opt => opt.Ignore())
            .ForMember(d => d.ImportCountry, opt => opt.Ignore())
            .ForMember(d => d.ExportCountry, opt => opt.Ignore());
    }

    private static GcModels.TradeParty MapConsign(IReadOnlyCollection<IdcomsModels.Organisation> source, GcModels.TradeParty consign)
    {
        if (source == null)
            return null;

        string organisationId = consign?.Id?.FirstOrDefault()?.Content;

        if (organisationId == null)
            return null;

        var establishment = source.FirstOrDefault(s => s.DefraCustomerId.ToString().Equals(organisationId, StringComparison.OrdinalIgnoreCase));

        if (establishment == null)
            return null;

        return new GcModels.TradeParty
        {
            Id = GetIdFromOrganisation(establishment),
            Name = GetNameFromOrganisation(establishment),
            RoleCode = GetRoleCode(),
            PostalAddress = GetConsignAddress(establishment)
        };
    }

    private static GcModels.LogisticsLocation MapLocation(IReadOnlyCollection<IdcomsModels.Establishment> source, GcModels.LogisticsLocation location)
    {
        if (source == null)
            return null;

        string dispatchLocationId = location?.Id?.FirstOrDefault()?.Content;

        if (dispatchLocationId == null)
            return null;

        var establishment = source.FirstOrDefault(s => s.EstablishmentId.ToString().Equals(dispatchLocationId, StringComparison.OrdinalIgnoreCase));

        if (establishment == null)
            return null;

        return new GcModels.LogisticsLocation
        {
            Id = GetId(establishment),
            Name = GetLocationName(establishment),
            LocationAddress = GetLocationAddress(establishment),
        };
    }

    private static List<GcModels.IDType> GetId(IdcomsModels.Establishment establishment)
    {
        return
        [
            new()
            {
                Content = establishment.RmsId,
                SchemeId = "RMS"
            },
            new()
            {
                Content = establishment.DefraCustomerId.ToString(),
                SchemeId = "Defra.Customer"
            }
        ];
    }

    private static List<GcModels.IDType> GetIdFromOrganisation(IdcomsModels.Organisation organisation)
    {
        return
                   [
                       new()
                           {
                               Content = organisation.RmsId,
                               SchemeId = "RMS"
                           },
                       new()
                           {
                               Content = organisation.DefraCustomerId.ToString(),
                               SchemeId = "Defra.Customer"
                           }
                   ];
    }

    private static List<GcModels.TextType> GetNameFromOrganisation(IdcomsModels.Organisation organisation)
    {
        return
                   [
                       new ()
                           {
                               Content = organisation.Name,
                               LanguageId = "en"
                           }
                   ];
    }

    private static GcModels.TextType GetLocationName(IdcomsModels.Establishment establishment)
    {
        return new GcModels.TextType
        {
            Content = establishment.Name,
            LanguageId = "en"
        };
    }

    private static List<GcModels.CodeType> GetRoleCode()
    {
        return
        [
            new ()
            {
                Content = "DGP"
            }
        ];
    }

    private static GcModels.TradeAddress GetConsignAddress(IdcomsModels.Organisation organisation)
    {
        var address = GetTraderAddressFromOrganisation(organisation);
        address.TypeCode = GetConsignTypeCode();

        return address;
    }

    private static GcModels.TradeAddress GetLocationAddress(IdcomsModels.Establishment establishment)
    {
        var address = GetTraderAddress(establishment);
        address.TypeCode = GetLocationTypeCode();

        return address;
    }

    private static GcModels.CodeType GetConsignTypeCode()
    {
        return new()
        {
            Content = "1",
            ListAgencyID = "6"
        };
    }

    private static GcModels.CodeType GetLocationTypeCode()
    {
        return new()
        {
            Content = "3",
            ListAgencyID = "6"
        };
    }

    private static GcModels.TradeAddress GetTraderAddress(IdcomsModels.Establishment establishment)
    {
        return new()
        {
            Postcode = new()
            {
                Content = establishment.Address.PostCode
            },
            LineOne = new()
            {
                Content = establishment.Address.AddressLine1
            },
            LineTwo = new()
            {
                Content = establishment.Address.AddressLine2
            },
            LineThree = new()
            {
                Content = establishment.Address.AddressLine3
            },
            LineFour = new()
            {
                Content = establishment.Address.AddressLine4
            },
            LineFive = new()
            {
                Content = establishment.Address.AddressLine5
            },
            CityName = new()
            {
                Content = establishment.Address.Town
            },
            CountryCode = new()
            {
                Content = establishment.Address.Country.Code,
                SchemeAgencyId = "5"
            },
            CountryName = new()
            {
                Content = establishment.Address.Country.Name,
                LanguageId = "en"
            },
            CountrySubDivisionCode = new()
            {
                Content = establishment.Address.Country.SubDivisionCode,
                SchemeAgencyId = "5"
            },
            CountrySubDivisionName = new()
            {
                Content = establishment.Address.Country.SubDivisionName,
                LanguageId = "en"
            }
        };
    }

    private static GcModels.TradeAddress GetTraderAddressFromOrganisation(IdcomsModels.Organisation organisation)
    {
        return new()
        {
            Postcode = new()
            {
                Content = organisation.Address.PostCode
            },
            LineOne = new()
            {
                Content = organisation.Address.AddressLine1
            },
            LineTwo = new()
            {
                Content = organisation.Address.AddressLine2
            },
            LineThree = new()
            {
                Content = organisation.Address.AddressLine3
            },
            LineFour = new()
            {
                Content = organisation.Address.AddressLine4
            },
            LineFive = new()
            {
                Content = organisation.Address.AddressLine5
            },
            CityName = new()
            {
                Content = organisation.Address.Town
            },
            CountryCode = new()
            {
                Content = organisation.Address.Country.Code,
                SchemeAgencyId = "5"
            },
            CountryName = new()
            {
                Content = organisation.Address.Country.Name,
                LanguageId = "en"
            },
            CountrySubDivisionCode = new()
            {
                Content = organisation.Address.Country.SubDivisionCode,
                SchemeAgencyId = "5"
            },
            CountrySubDivisionName = new()
            {
                Content = organisation.Address.Country.SubDivisionName,
                LanguageId = "en"
            }
        };
    }
}