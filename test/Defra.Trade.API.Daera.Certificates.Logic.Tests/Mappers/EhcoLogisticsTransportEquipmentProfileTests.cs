// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Defra.Trade.API.Daera.Certificates.Logic.Mappers;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Mappers;

public class EhcoLogisticsTransportEquipmentProfileTests
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
        var source = new EhcoModels.LogisticsTransportEquipment
        {
            AffixedSeal = "SEAL0001",
            TemperatureSetting = "2",
            TrailerNumber = "TRL-0002_001"
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<IList<GcModels.LogisticsTransportEquipment>>(source);

        // Assert 
        result.Should().NotBeNull();

        var equipment = result.Should().HaveCount(1).And.Subject.First();
        equipment.Id.Content.Should().Be("TRL-0002_001");
        equipment.AffixedSeal.Should().HaveCount(1).And.Subject.First().Id.Content.Should().Be("SEAL0001");
        var temperature = equipment.TemperatureSetting.Should().HaveCount(1).And.Subject.First();
        temperature.Value.Content.Should().Be(2);
        temperature.TypeCode.Content.Should().Be("2");
        temperature.TypeCode.ListAgencyID.Should().Be("6");
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
            cfg.AddProfile<EhcoLogisticsTransportEquipmentProfile>();
        });
    }
}