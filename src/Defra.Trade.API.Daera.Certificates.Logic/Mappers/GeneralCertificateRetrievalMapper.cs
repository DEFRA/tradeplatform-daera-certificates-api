// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoMapper;
using JsonSerializerOptions = Defra.Trade.API.Daera.Certificates.Logic.SerializerOptions.SerializerOptions;

namespace Defra.Trade.API.Daera.Certificates.Logic.Mappers;

public class GeneralCertificateRetrievalMapper : Profile
{
    public GeneralCertificateRetrievalMapper()
    {
        CreateMap<Database.Models.GeneralCertificate, Models.Ehco.EhcoGeneralCertificateApplication>()
            .ConvertUsing((s, d) =>
                JsonSerializer.Deserialize<Models.Ehco.EhcoGeneralCertificateApplication>(s.Data, JsonSerializerOptions.GetSerializerOptions()));
    }
}