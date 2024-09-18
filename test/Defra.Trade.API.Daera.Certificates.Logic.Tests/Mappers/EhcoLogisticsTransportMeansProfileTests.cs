// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Defra.Trade.API.Daera.Certificates.Logic.Mappers;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Mappers;

public class EhcoLogisticsTransportMeansProfileTests
{
    [Fact]
    public void Mapper_Configuration_IsValid()
    {
        var config = CreateConfiguration();

        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_ConvertFrom_IsValid()
    {
        // Arrange
        var source = new EhcoModels.LogisticsTransportMeans
        {
            ID = "MEANS-XYZ-00002",
            ModeCode = "3"
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<IList<GcModels.LogisticsTransportMovement>>(source);

        // Assert 
        result.Should().NotBeNull();

        var movement = result.Should().HaveCount(1).And.Subject.First();
        movement.ModeCode.Content.Should().Be("3");
        movement.ModeCode.ListAgencyID.Should().Be("6");

        movement.UsedTransportMeans.Id.Content.Should().Be("MEANS-XYZ-00002");
    }

    private static IMapper CreateSut()
    {
        var config = CreateConfiguration();

        return config.CreateMapper();
    }

    private static MapperConfiguration CreateConfiguration()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<EhcoLogisticsTransportMeansProfile>();
        });
    }
}