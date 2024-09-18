// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Linq;
using AutoMapper;
using Defra.Trade.API.Daera.Certificates.Logic.Mappers;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Mappers;

public class EhcoOperatorProfileTests
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
        var source = new EhcoModels.Operator()
        {
            Name = "Operator1",
            Postcode = "P57 CDE",
            LineOne = "Test line 1",
            LineTwo = "Test line 2",
            CityName = "Test City",
            CountryCode = "GB",
            Telephone = "01234567890",
            Traces = new EhcoModels.OperatorTraces
            {
                OperatorId = "rflid"
            }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<GcModels.TradeParty>(source);

        // Assert 
        result.Should().NotBeNull();

        var name = result.Name.Should().HaveCount(1).And.Subject.First();
        name.Content.Should().Be("Operator1");
        name.LanguageId.Should().Be("en");

        result.RoleCode.Should().HaveCount(1).And.Subject.First().Content.Should().Be("AG");

        result.PostalAddress.Postcode.Content.Should().Be("P57 CDE");
        result.PostalAddress.LineOne.Content.Should().Be("Test line 1");
        result.PostalAddress.LineTwo.Content.Should().Be("Test line 2");
        result.PostalAddress.LineThree.Should().BeNull();
        result.PostalAddress.LineFour.Should().BeNull();
        result.PostalAddress.LineFive.Should().BeNull();
        result.PostalAddress.CityName.Content.Should().Be("Test City");
        result.PostalAddress.CountryCode.Content.Should().Be("GB");
        result.PostalAddress.CountryCode.SchemeAgencyId.Should().Be("5");
        result.PostalAddress.CountryName.Should().BeNull();
        result.PostalAddress.CountrySubDivisionCode.Should().BeNull();
        result.PostalAddress.CountrySubDivisionName.Should().BeNull();
        result.PostalAddress.TypeCode.Content.Should().Be("1");
        result.PostalAddress.TypeCode.ListAgencyID.Should().Be("6");

        result.Telephone.CompleteNumber.Content.Should().Be("01234567890");
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
            cfg.AddProfile<EhcoOperatorProfile>();
        });
    }
}