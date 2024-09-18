// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using LogicModels = Defra.Trade.API.Daera.Certificates.Logic.Models;

namespace Defra.Trade.API.Daera.Certificates.V1.Mappers;

public class TemperatureUnitMeasureProfile : Profile
{
    public TemperatureUnitMeasureProfile()
    {
        CreateMap<LogicModels.GeneralCertificate.TemperatureUnitMeasure, Dtos.GeneralCertificate.TemperatureUnitMeasure>();
    }
}