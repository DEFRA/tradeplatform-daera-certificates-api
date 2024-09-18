// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoMapper;
using JsonSerializerOptions = Defra.Trade.API.Daera.Certificates.Logic.SerializerOptions.SerializerOptions;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class EnrichmentDataRetrievalMapper : Profile
{
    public EnrichmentDataRetrievalMapper()
    {
        CreateMap<Database.Models.GeneralCertificate, Models.Idcoms.IdcomsGeneralCertificateEnrichment>()
            .ConvertUsing((s, d) =>
                JsonSerializer.Deserialize<Models.Idcoms.IdcomsGeneralCertificateEnrichment>(s.EnrichmentData.Data, JsonSerializerOptions.GetSerializerOptions()));
    }
}