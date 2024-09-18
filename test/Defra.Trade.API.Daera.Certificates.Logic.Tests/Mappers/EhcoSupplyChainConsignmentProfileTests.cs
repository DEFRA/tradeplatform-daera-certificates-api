// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Linq;
using AutoMapper;
using Defra.Trade.API.Daera.Certificates.Logic.Mappers;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Mappers;

public class EhcoSupplyChainConsignmentProfileTests
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
        var consignorOrgId = Guid.NewGuid();
        var consigneeOrgId = Guid.NewGuid();
        var destinationLocationId = Guid.NewGuid();
        var dispatchLocationId = Guid.NewGuid();

        var source = new EhcoModels.SupplyChainConsignment
        {
            CustomsId = "GOODSMOVEMENT123",
            ExportExitDateTime = "06/07/2023",
            Consignor = new EhcoModels.Consignor
            {
                DefraCustomer = new()
                {
                    OrgId = consignorOrgId
                }
            },
            Consignee = new EhcoModels.Consignee
            {
                DefraCustomer = new()
                {
                    OrgId = consigneeOrgId
                }
            },
            OperatorResponsibleForConsignment = new EhcoModels.Operator
            {
                Name = "Test name"
            },
            BorderControlPostLocation = "BCP123",
            DispatchLocation = new EhcoModels.Location
            {
                Idcoms = new()
                {
                    EstablishmentId = dispatchLocationId
                }
            },
            DestinationLocation = new EhcoModels.Location
            {
                Idcoms = new()
                {
                    EstablishmentId = destinationLocationId
                }
            },
            ExportCountry = "GB",
            ImportCountry = "XI",
            UsedTransportEquipment = new EhcoModels.LogisticsTransportEquipment
            {
                TrailerNumber = "TRL-0002_001"
            },
            UsedTransportMeans = new EhcoModels.LogisticsTransportMeans
            {
                ID = "MEANS-ID-123"
            }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<GcModels.SupplyChainConsignment>(source);

        // Assert
        result.Should().NotBeNull();

        result.Id.Should().BeNull();

        var customsId = result.CustomsId.Should().HaveCount(1).And.Subject.First();
        customsId.Content.Should().Be("GOODSMOVEMENT123");
        customsId.SchemeId.Should().Be("GMR");

        result.ExportExitDateTime.Content.Should().Be("202307060000+00:00");
        result.ExportExitDateTime.Format.Should().Be("205");

        var consignorId = result.Consignor.Id.Should().HaveCount(1).And.Subject.First();
        consignorId.Content.Should().Be(consignorOrgId.ToString());

        var consigneeId = result.Consignee.Id.Should().HaveCount(1).And.Subject.First();
        consigneeId.Content.Should().Be(consigneeOrgId.ToString());

        result.OperatorResponsibleForConsignment.Name.Should().HaveCount(1).And.Subject.First().Content.Should()
            .Be("Test name");

        var bcpId = result.BorderControlPostLocation.Id.Should().HaveCount(1).And.Subject.First();
        bcpId.Content.Should().Be("BCP123");
        bcpId.SchemeId.Should().Be("BCP");

        var dispatchId = result.DispatchLocation.Id.Should().HaveCount(1).And.Subject.First();
        dispatchId.Content.Should().Be(dispatchLocationId.ToString());

        var destinationId = result.DestinationLocation.Id.Should().HaveCount(1).And.Subject.First();
        destinationId.Content.Should().Be(destinationLocationId.ToString());

        result.OriginCountry.Should().BeNull();

        result.ExportCountry.Code.Content.Should().Be("GB");
        result.ExportCountry.Code.SchemeAgencyId.Should().Be("5");

        result.ImportCountry.Code.Content.Should().Be("XI");
        result.ImportCountry.Code.SchemeAgencyId.Should().Be("5");

        result.UsedTransportEquipment.Should().HaveCount(1).And.Subject.First().Id.Content.Should().Be("TRL-0002_001");

        result.MainCarriageTransportMovement.Should().HaveCount(1).And.Subject.First().UsedTransportMeans.Id.Content.Should().Be("MEANS-ID-123");
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
            cfg.AddProfile<EhcoSupplyChainConsignmentProfile>();
            cfg.AddProfile<EhcoOperatorProfile>();
            cfg.AddProfile<EhcoLogisticsTransportEquipmentProfile>();
            cfg.AddProfile<EhcoLogisticsTransportMeansProfile>();
        });
    }
}