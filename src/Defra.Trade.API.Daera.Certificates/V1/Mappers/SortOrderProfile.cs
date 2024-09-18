// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using LogicModels = Defra.Trade.API.Daera.Certificates.Logic.Models;

namespace Defra.Trade.API.Daera.Certificates.V1.Mappers;

public class SortOrderProfile : Profile
{
    public SortOrderProfile()
    {
        CreateMap<Dtos.Enums.SortOrder, LogicModels.Enums.SortOrder>()
            .ConvertUsing(s => ConvertSortOrder(s));
    }

    private static LogicModels.Enums.SortOrder ConvertSortOrder(Dtos.Enums.SortOrder source)
    {
        return source switch
        {
            Dtos.Enums.SortOrder.Desc => LogicModels.Enums.SortOrder.Desc,
            _ => LogicModels.Enums.SortOrder.Asc
        };
    }
}