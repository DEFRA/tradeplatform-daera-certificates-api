// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Globalization;
using AutoMapper;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class EhcoSupplyChainConsignmentProfile : Profile
{
    public EhcoSupplyChainConsignmentProfile()
    {
        CreateMap<EhcoModels.SupplyChainConsignment, GcModels.SupplyChainConsignment>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CustomsId,
                opt => opt.MapFrom(s => MapCustomsId(s.CustomsId)))
            .ForMember(d => d.ExportExitDateTime, opt => opt.MapFrom(s => MapExportExitDateTime(s.ExportExitDateTime)))
            .ForMember(d => d.Consignor, opt => opt.MapFrom(s => MapTradePartyOrgId(s.Consignor.DefraCustomer.OrgId)))
            .ForMember(d => d.Consignee, opt => opt.MapFrom(s => MapTradePartyOrgId(s.Consignee.DefraCustomer.OrgId)))
            .ForMember(d => d.OperatorResponsibleForConsignment,
                opt => opt.MapFrom(s => s.OperatorResponsibleForConsignment))
            .ForMember(d => d.BorderControlPostLocation,
                opt => opt.MapFrom(s => MapBorderControlPost(s.BorderControlPostLocation)))
            .ForMember(d => d.DispatchLocation, opt => opt.MapFrom(s => MapLogisticsEstablishmentId(s.DispatchLocation.Idcoms.EstablishmentId)))
            .ForMember(d => d.DestinationLocation, opt => opt.MapFrom(s => MapLogisticsEstablishmentId(s.DestinationLocation.Idcoms.EstablishmentId)))
            .ForMember(d => d.OriginCountry, opt => opt.Ignore())
            .ForMember(d => d.ExportCountry, opt => opt.MapFrom(s => MapCountry(s.ExportCountry)))
            .ForMember(d => d.ImportCountry, opt => opt.MapFrom(s => MapCountry(s.ImportCountry)))
            .ForMember(d => d.UsedTransportEquipment, opt => opt.MapFrom(s => s.UsedTransportEquipment))
            .ForMember(d => d.MainCarriageTransportMovement, opt => opt.MapFrom(s => s.UsedTransportMeans));
    }

    private static List<GcModels.IDType> MapCustomsId(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return [];

        return
        [
            MapId(source, "GMR")
        ];
    }

    private static GcModels.IDType MapId(string source, string schemeId)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.IDType
        {
            Content = source,
            SchemeId = schemeId
        };
    }

    private static GcModels.DateTimeType MapExportExitDateTime(string source)
    {
        if (string.IsNullOrWhiteSpace(source)
            || !DateTimeOffset.TryParseExact(source,
                "dd/MM/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal, out var parsed))
        {
            return null;
        }

        return new GcModels.DateTimeType
        {
            Content = parsed.ToString("yyyyMMddHHmmzzz"),
            Format = "205"
        };
    }

    private static GcModels.TradeParty MapTradePartyOrgId(Guid orgId)
    {
        if (orgId == Guid.Empty)
            return null;

        return new GcModels.TradeParty
        {
            Id =
            [
                new()
                {
                    Content = orgId.ToString(),
                }
            ]
        };
    }

    private static GcModels.LogisticsLocation MapLogisticsEstablishmentId(Guid establishmentId)
    {
        if (establishmentId == Guid.Empty)
            return null;

        return new GcModels.LogisticsLocation
        {
            Id =
            [
                new()
                {
                    Content = establishmentId.ToString(),
                }
            ]
        };
    }

    private static GcModels.LogisticsLocation MapBorderControlPost(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.LogisticsLocation
        {
            Id =
            [
                new()
                {
                    Content = source,
                    SchemeId = "BCP"
                }
            ]
        };
    }

    private static GcModels.TradeCountry MapCountry(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.TradeCountry
        {
            Code = new GcModels.IDType
            {
                Content = source,
                SchemeAgencyId = "5"
            }
        };
    }
}