// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using DBModels = Defra.Trade.API.Daera.Certificates.Database;

namespace Defra.Trade.API.Daera.Certificates.V1.Mappers;

public class GeneralCertificateSummariesQuery : Profile
{
    public GeneralCertificateSummariesQuery()
    {
        CreateMap<Queries.GetGeneralCertificateSummariesQuery, DBModels.Models.GeneralCertificateSummariesQuery>();
    }
}