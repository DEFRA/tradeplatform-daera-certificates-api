// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoMapper;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class EhcoGeneralCertificateProfile : Profile
{
    public EhcoGeneralCertificateProfile()
    {
        CreateMap<EhcoModels.EhcoGeneralCertificateApplication, GcModels.GeneralCertificate>()
            .ForMember(d => d.ExchangedDocument, opt => opt.MapFrom(s => s.ExchangedDocument))
            .ForMember(d => d.SupplyChainConsignment, opt => opt.MapFrom(s => s.SupplyChainConsignment));
    }
}