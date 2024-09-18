// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using DBModels = Defra.Trade.API.Daera.Certificates.Database;
using LogicModels = Defra.Trade.API.Daera.Certificates.Logic.Models;

namespace Defra.Trade.API.Daera.Certificates.V1.Mappers;

public class PagedGeneralCertificateSummaryProfile : Profile
{
    public PagedGeneralCertificateSummaryProfile()
    {
        CreateMap<DBModels.Models.PagedGeneralCertificateSummary, LogicModels.PagedGeneralCertificateSummary>();
    }
}