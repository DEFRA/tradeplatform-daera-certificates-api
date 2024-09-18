// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using LogicModels = Defra.Trade.API.Daera.Certificates.Logic.Models;

namespace Defra.Trade.API.Daera.Certificates.V1.Mappers;

public class IDTypeProfile : Profile
{
    public IDTypeProfile()
    {
        CreateMap<LogicModels.GeneralCertificate.IDType, Dtos.GeneralCertificate.IDType>();
    }
}