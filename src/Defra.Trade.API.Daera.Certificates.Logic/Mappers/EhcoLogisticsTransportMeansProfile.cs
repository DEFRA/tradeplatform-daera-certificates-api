// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoMapper;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class EhcoLogisticsTransportMeansProfile : Profile
{
    public EhcoLogisticsTransportMeansProfile()
    {
        CreateMap<EhcoModels.LogisticsTransportMeans, IList<GcModels.LogisticsTransportMovement>>()
            .ConvertUsing((s, d, c) => [c.Mapper.Map<GcModels.LogisticsTransportMovement>(s)]);

        CreateMap<EhcoModels.LogisticsTransportMeans, GcModels.LogisticsTransportMovement>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.ModeCode, opt => opt.MapFrom(s => MapModeCode(s.ModeCode)))
            .ForMember(d => d.UsedTransportMeans, opt => opt.MapFrom(s => MapMeans(s.ID)));
    }

    private static GcModels.CodeType MapModeCode(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.CodeType
        {
            Content = source,
            ListAgencyID = "6"
        };
    }

    private static GcModels.LogisticsTransportMeans MapMeans(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.LogisticsTransportMeans
        {
            Id = new GcModels.IDType
            {
                Content = source
            }
        };
    }
}