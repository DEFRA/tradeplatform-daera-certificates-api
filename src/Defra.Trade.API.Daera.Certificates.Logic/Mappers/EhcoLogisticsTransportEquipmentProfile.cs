// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoMapper;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class EhcoLogisticsTransportEquipmentProfile : Profile
{
    public EhcoLogisticsTransportEquipmentProfile()
    {
        CreateMap<EhcoModels.LogisticsTransportEquipment, IList<GcModels.LogisticsTransportEquipment>>()
            .ConvertUsing((s, d, c) => [c.Mapper.Map<GcModels.LogisticsTransportEquipment>(s)]);

        CreateMap<EhcoModels.LogisticsTransportEquipment, GcModels.LogisticsTransportEquipment>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => MapTrailerNumber(s.TrailerNumber)))
            .ForMember(d => d.AffixedSeal, opt => opt.MapFrom(s => MapAffixedSeal(s.AffixedSeal)))
            .ForMember(d => d.TemperatureSetting, opt => opt.MapFrom(s => MapTemperature(s.TemperatureSetting)));
    }

    private static GcModels.IDType MapTrailerNumber(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.IDType
        {
            Content = source
        };
    }

    private static List<GcModels.LogisticsSeal> MapAffixedSeal(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return [];

        return
        [
            new()
            {
                Id = new GcModels.IDType
                {
                    Content = source
                }
            }
        ];
    }

    private static List<GcModels.TransportSettingTemperature> MapTemperature(string source)
    {
        if (string.IsNullOrWhiteSpace(source)
            || !decimal.TryParse(source, out decimal parsed))
        {
            return [];
        }

        return
        [
            new()
            {
                Value = new GcModels.TemperatureUnitMeasure
                {
                    Content = parsed
                },
                TypeCode = new GcModels.CodeType
                {
                    Content = "2",
                    ListAgencyID = "6"
                }
            }
        ];
    }
}