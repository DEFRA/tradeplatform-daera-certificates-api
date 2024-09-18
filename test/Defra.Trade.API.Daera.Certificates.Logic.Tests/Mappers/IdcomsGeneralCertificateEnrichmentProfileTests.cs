// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Generic;
using AutoFixture;
using AutoMapper;
using Defra.Trade.API.Daera.Certificates.Logic.Mappers;
using Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Mappers;

public class IdcomsGeneralCertificateEnrichmentProfileTests
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
        var consigneeId = Guid.NewGuid();
        var consignorId = Guid.NewGuid();
        var destinationLocationId = Guid.NewGuid();
        var dispatchLocationId = Guid.NewGuid();

        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consigneeId.ToString() }])
                .Create())
            .With(s => s.Consignor, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consignorId.ToString() }])
                .Create())
            .With(s => s.DestinationLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = destinationLocationId.ToString() }])
                .Create())
            .With(s => s.DispatchLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = dispatchLocationId.ToString() }])
                .Create())
            .Create();

        var consignee = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consigneeId)
            .Create();
        var consignor = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consignorId)
            .Create();
        var destinationLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, destinationLocationId)
            .Create();
        var dispatchLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, dispatchLocationId)
            .Create();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { consignee, consignor },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { destinationLocation, dispatchLocation }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().NotBeNull();
        result.Consignee.Id.Should().Contain(x => x.Content.Equals(consignee.RmsId.ToString()));
        result.Consignee.Id.Should().Contain(x => x.SchemeId.Equals("RMS"));
        result.Consignee.Id.Should().Contain(x => x.Content.Equals(consignee.DefraCustomerId.ToString()));
        result.Consignee.Id.Should().Contain(x => x.SchemeId.Equals("Defra.Customer"));

        result.Consignee.Name.Should().Contain(x => x.Content.Equals(consignee.Name));
        result.Consignee.Name.Should().Contain(x => x.LanguageId.Equals("en"));

        result.Consignee.RoleCode.Should().Contain(x => x.Content.Equals("DGP"));

        var consigneePostalAddress = result.Consignee.PostalAddress;
        consigneePostalAddress.Postcode.Content.Should().Be(consignee.Address.PostCode);
        consigneePostalAddress.LineOne.Content.Should().Be(consignee.Address.AddressLine1);
        consigneePostalAddress.LineTwo.Content.Should().Be(consignee.Address.AddressLine2);
        consigneePostalAddress.LineThree.Content.Should().Be(consignee.Address.AddressLine3);
        consigneePostalAddress.LineFour.Content.Should().Be(consignee.Address.AddressLine4);
        consigneePostalAddress.LineFive.Content.Should().Be(consignee.Address.AddressLine5);
        consigneePostalAddress.CityName.Content.Should().Be(consignee.Address.Town);
        consigneePostalAddress.CountryCode.Content.Should().Be(consignee.Address.Country.Code);
        consigneePostalAddress.CountryCode.SchemeAgencyId.Should().Be("5");
        consigneePostalAddress.CountryName.Content.Should().Be(consignee.Address.Country.Name);
        consigneePostalAddress.CountryName.LanguageId.Should().Be("en");
        consigneePostalAddress.CountrySubDivisionCode.Content.Should().Be(consignee.Address.Country.SubDivisionCode);
        consigneePostalAddress.CountrySubDivisionCode.SchemeAgencyId.Should().Be("5");
        consigneePostalAddress.CountrySubDivisionName.Content.Should().Be(consignee.Address.Country.SubDivisionName);
        consigneePostalAddress.CountrySubDivisionName.LanguageId.Should().Be("en");
        consigneePostalAddress.TypeCode.Content.Should().Be("1");
        consigneePostalAddress.TypeCode.ListAgencyID.Should().Be("6");

        result.Consignor.Should().NotBeNull();
        result.Consignor.Id.Should().Contain(x => x.Content.Equals(consignor.RmsId.ToString()));
        result.Consignor.Id.Should().Contain(x => x.SchemeId.Equals("RMS"));
        result.Consignor.Id.Should().Contain(x => x.Content.Equals(consignor.DefraCustomerId.ToString()));
        result.Consignor.Id.Should().Contain(x => x.SchemeId.Equals("Defra.Customer"));

        result.Consignor.Name.Should().Contain(x => x.Content.Equals(consignor.Name));
        result.Consignor.Name.Should().Contain(x => x.LanguageId.Equals("en"));

        result.Consignor.RoleCode.Should().Contain(x => x.Content.Equals("DGP"));

        var consignorPostalAddress = result.Consignor.PostalAddress;
        consignorPostalAddress.Postcode.Content.Should().Be(consignor.Address.PostCode);
        consignorPostalAddress.LineOne.Content.Should().Be(consignor.Address.AddressLine1);
        consignorPostalAddress.LineTwo.Content.Should().Be(consignor.Address.AddressLine2);
        consignorPostalAddress.LineThree.Content.Should().Be(consignor.Address.AddressLine3);
        consignorPostalAddress.LineFour.Content.Should().Be(consignor.Address.AddressLine4);
        consignorPostalAddress.LineFive.Content.Should().Be(consignor.Address.AddressLine5);
        consignorPostalAddress.CityName.Content.Should().Be(consignor.Address.Town);
        consignorPostalAddress.CountryCode.Content.Should().Be(consignor.Address.Country.Code);
        consignorPostalAddress.CountryCode.SchemeAgencyId.Should().Be("5");
        consignorPostalAddress.CountryName.Content.Should().Be(consignor.Address.Country.Name);
        consignorPostalAddress.CountryName.LanguageId.Should().Be("en");
        consignorPostalAddress.CountrySubDivisionCode.Content.Should().Be(consignor.Address.Country.SubDivisionCode);
        consignorPostalAddress.CountrySubDivisionCode.SchemeAgencyId.Should().Be("5");
        consignorPostalAddress.CountrySubDivisionName.Content.Should().Be(consignor.Address.Country.SubDivisionName);
        consignorPostalAddress.CountrySubDivisionName.LanguageId.Should().Be("en");
        consignorPostalAddress.TypeCode.Content.Should().Be("1");
        consignorPostalAddress.TypeCode.ListAgencyID.Should().Be("6");

        result.DestinationLocation.Should().NotBeNull();
        result.DestinationLocation.Id.Should().Contain(x => x.Content.Equals(destinationLocation.RmsId.ToString()));
        result.DestinationLocation.Id.Should().Contain(x => x.SchemeId.Equals("RMS"));
        result.DestinationLocation.Id.Should().Contain(x => x.Content.Equals(destinationLocation.DefraCustomerId.ToString()));
        result.DestinationLocation.Id.Should().Contain(x => x.SchemeId.Equals("Defra.Customer"));

        result.DestinationLocation.Name.Content.Should().Be(destinationLocation.Name);
        result.DestinationLocation.Name.LanguageId.Should().Be("en");

        var destinationLocationAddress = result.DestinationLocation.LocationAddress;
        destinationLocationAddress.Postcode.Content.Should().Be(destinationLocation.Address.PostCode);
        destinationLocationAddress.LineOne.Content.Should().Be(destinationLocation.Address.AddressLine1);
        destinationLocationAddress.LineTwo.Content.Should().Be(destinationLocation.Address.AddressLine2);
        destinationLocationAddress.LineThree.Content.Should().Be(destinationLocation.Address.AddressLine3);
        destinationLocationAddress.LineFour.Content.Should().Be(destinationLocation.Address.AddressLine4);
        destinationLocationAddress.LineFive.Content.Should().Be(destinationLocation.Address.AddressLine5);
        destinationLocationAddress.CityName.Content.Should().Be(destinationLocation.Address.Town);
        destinationLocationAddress.CountryCode.Content.Should().Be(destinationLocation.Address.Country.Code);
        destinationLocationAddress.CountryCode.SchemeAgencyId.Should().Be("5");
        destinationLocationAddress.CountryName.Content.Should().Be(destinationLocation.Address.Country.Name);
        destinationLocationAddress.CountryName.LanguageId.Should().Be("en");
        destinationLocationAddress.CountrySubDivisionCode.Content.Should().Be(destinationLocation.Address.Country.SubDivisionCode);
        destinationLocationAddress.CountrySubDivisionCode.SchemeAgencyId.Should().Be("5");
        destinationLocationAddress.CountrySubDivisionName.Content.Should().Be(destinationLocation.Address.Country.SubDivisionName);
        destinationLocationAddress.CountrySubDivisionName.LanguageId.Should().Be("en");
        destinationLocationAddress.TypeCode.Content.Should().Be("3");
        destinationLocationAddress.TypeCode.ListAgencyID.Should().Be("6");

        result.DispatchLocation.Should().NotBeNull();
        result.DispatchLocation.Id.Should().Contain(x => x.Content.Equals(dispatchLocation.RmsId.ToString()));
        result.DispatchLocation.Id.Should().Contain(x => x.SchemeId.Equals("RMS"));
        result.DispatchLocation.Id.Should().Contain(x => x.Content.Equals(dispatchLocation.DefraCustomerId.ToString()));
        result.DispatchLocation.Id.Should().Contain(x => x.SchemeId.Equals("Defra.Customer"));

        result.DispatchLocation.Name.Content.Should().Be(dispatchLocation.Name);
        result.DispatchLocation.Name.LanguageId.Should().Be("en");

        var dispatchLocationAddress = result.DispatchLocation.LocationAddress;
        dispatchLocationAddress.Postcode.Content.Should().Be(dispatchLocation.Address.PostCode);
        dispatchLocationAddress.LineOne.Content.Should().Be(dispatchLocation.Address.AddressLine1);
        dispatchLocationAddress.LineTwo.Content.Should().Be(dispatchLocation.Address.AddressLine2);
        dispatchLocationAddress.LineThree.Content.Should().Be(dispatchLocation.Address.AddressLine3);
        dispatchLocationAddress.LineFour.Content.Should().Be(dispatchLocation.Address.AddressLine4);
        dispatchLocationAddress.LineFive.Content.Should().Be(dispatchLocation.Address.AddressLine5);
        dispatchLocationAddress.CityName.Content.Should().Be(dispatchLocation.Address.Town);
        dispatchLocationAddress.CountryCode.Content.Should().Be(dispatchLocation.Address.Country.Code);
        dispatchLocationAddress.CountryCode.SchemeAgencyId.Should().Be("5");
        dispatchLocationAddress.CountryName.Content.Should().Be(dispatchLocation.Address.Country.Name);
        dispatchLocationAddress.CountryName.LanguageId.Should().Be("en");
        dispatchLocationAddress.CountrySubDivisionCode.Content.Should().Be(dispatchLocation.Address.Country.SubDivisionCode);
        dispatchLocationAddress.CountrySubDivisionCode.SchemeAgencyId.Should().Be("5");
        dispatchLocationAddress.CountrySubDivisionName.Content.Should().Be(dispatchLocation.Address.Country.SubDivisionName);
        dispatchLocationAddress.CountrySubDivisionName.LanguageId.Should().Be("en");
        dispatchLocationAddress.TypeCode.Content.Should().Be("3");
        dispatchLocationAddress.TypeCode.ListAgencyID.Should().Be("6");
    }

    [Fact]
    public void Map_ConvertFrom_Establishments_EmptyList_Returns_No_Consignor_No_Consignee_No_DispatchLocation_No_DestinationLocation()
    {
        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Build<GcModels.TradeParty>()
                .Create())
            .With(s => s.Consignor, new Fixture()
                .Build<GcModels.TradeParty>()
                .Create())
            .With(s => s.DestinationLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .Create())
            .With(s => s.DispatchLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .Create())
            .Create();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().BeNull();
        result.Consignor.Should().BeNull();
        result.DestinationLocation.Should().BeNull();
        result.DispatchLocation.Should().BeNull();
    }

    [Fact]
    public void Map_ConvertFrom_Establishments_With_No_Matching_ConsigneeId_Returns_No_Consignee()
    {
        // Arrange
        var consignorId = Guid.NewGuid();
        var destinationLocationId = Guid.NewGuid();
        var dispatchLocationId = Guid.NewGuid();

        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Create<GcModels.TradeParty>())
            .With(s => s.Consignor, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consignorId.ToString() }])
                .Create())
            .With(s => s.DestinationLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = destinationLocationId.ToString() }])
                .Create())
            .With(s => s.DispatchLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = dispatchLocationId.ToString() }])
                .Create())
            .Create();

        var consignee = new Fixture()
            .Create<Logic.Models.Idcoms.Organisation>();
        var consignor = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consignorId)
            .Create();
        var destinationLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, destinationLocationId)
            .Create();
        var dispatchLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, dispatchLocationId)
            .Create();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { consignee, consignor },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { destinationLocation, dispatchLocation }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().BeNull();
        result.Consignor.Should().NotBeNull();
        result.DestinationLocation.Should().NotBeNull();
        result.DispatchLocation.Should().NotBeNull();
    }

    [Fact]
    public void Map_ConvertFrom_Establishments_Where_ConsigneeId_IsNull_Returns_No_Consignee()
    {
        // Arrange
        var consignorId = Guid.NewGuid();
        var destinationLocationId = Guid.NewGuid();
        var dispatchLocationId = Guid.NewGuid();

        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = null }])
                .Create())
            .With(s => s.Consignor, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consignorId.ToString() }])
                .Create())
            .With(s => s.DestinationLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = destinationLocationId.ToString() }])
                .Create())
            .With(s => s.DispatchLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = dispatchLocationId.ToString() }])
                .Create())
            .Create();

        var consignor = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consignorId)
            .Create();
        var destinationLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, destinationLocationId)
            .Create();
        var dispatchLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, dispatchLocationId)
            .Create();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { consignor },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { destinationLocation, dispatchLocation }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().BeNull();
        result.Consignor.Should().NotBeNull();
        result.DestinationLocation.Should().NotBeNull();
        result.DispatchLocation.Should().NotBeNull();
    }

    [Fact]
    public void Map_ConvertFrom_Establishments_With_No_Matching_ConsignorId_Returns_No_Consignor()
    {
        // Arrange
        var consigneeId = Guid.NewGuid();
        var destinationLocationId = Guid.NewGuid();
        var dispatchLocationId = Guid.NewGuid();

        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consigneeId.ToString() }])
                .Create())
            .With(s => s.Consignor, new Fixture()
                .Create<GcModels.TradeParty>())
            .With(s => s.DestinationLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = destinationLocationId.ToString() }])
                .Create())
            .With(s => s.DispatchLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = dispatchLocationId.ToString() }])
                .Create())
            .Create();

        var consignee = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consigneeId)
            .Create();
        var consignor = new Fixture()
            .Create<Logic.Models.Idcoms.Organisation>();
        var destinationLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, destinationLocationId)
            .Create();
        var dispatchLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, dispatchLocationId)
            .Create();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { consignee, consignor },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { destinationLocation, dispatchLocation }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().NotBeNull();
        result.Consignor.Should().BeNull();
        result.DestinationLocation.Should().NotBeNull();
        result.DispatchLocation.Should().NotBeNull();
    }

    [Fact]
    public void Map_ConvertFrom_Establishments_Where_ConsignorId_IsNull_Returns_No_Consignor()
    {
        // Arrange
        var consigneeId = Guid.NewGuid();
        var destinationLocationId = Guid.NewGuid();
        var dispatchLocationId = Guid.NewGuid();

        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consigneeId.ToString() }])
                .Create())
            .With(s => s.Consignor, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = null }])
                .Create())
            .With(s => s.DestinationLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = destinationLocationId.ToString() }])
                .Create())
            .With(s => s.DispatchLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = dispatchLocationId.ToString() }])
                .Create())
            .Create();

        var consignee = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consigneeId)
            .Create();
        var destinationLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, destinationLocationId)
            .Create();
        var dispatchLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, dispatchLocationId)
            .Create();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { consignee },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { destinationLocation, dispatchLocation }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().NotBeNull();
        result.Consignor.Should().BeNull();
        result.DestinationLocation.Should().NotBeNull();
        result.DispatchLocation.Should().NotBeNull();
    }

    [Fact]
    public void Map_ConvertFrom_Establishments_With_No_Matching_DestinationLocationId_Returns_No_DestinationLocation()
    {
        // Arrange
        var consigneeId = Guid.NewGuid();
        var consignorId = Guid.NewGuid();
        var dispatchLocationId = Guid.NewGuid();

        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consigneeId.ToString() }])
                .Create())
            .With(s => s.Consignor, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consignorId.ToString() }])
                .Create())
            .With(s => s.DestinationLocation, new Fixture()
                .Create<GcModels.LogisticsLocation>())
            .With(s => s.DispatchLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = dispatchLocationId.ToString() }])
                .Create())
            .Create();

        var consignee = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consigneeId)
            .Create();
        var consignor = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consignorId)
            .Create();
        var destinationLocation = new Fixture()
            .Create<Logic.Models.Idcoms.Establishment>();
        var dispatchLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, dispatchLocationId)
            .Create();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { consignee, consignor },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { destinationLocation, dispatchLocation }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().NotBeNull();
        result.Consignor.Should().NotBeNull();
        result.DestinationLocation.Should().BeNull();
        result.DispatchLocation.Should().NotBeNull();
    }

    [Fact]
    public void Map_ConvertFrom_Establishments_Where_DestinationLocationId_IsNull_Returns_No_DestinationLocation()
    {
        // Arrange
        var consigneeId = Guid.NewGuid();
        var consignorId = Guid.NewGuid();
        var dispatchLocationId = Guid.NewGuid();

        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consigneeId.ToString() }])
                .Create())
            .With(s => s.Consignor, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consignorId.ToString() }])
                .Create())
            .With(s => s.DestinationLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = null }])
                .Create())
            .With(s => s.DispatchLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = dispatchLocationId.ToString() }])
                .Create())
            .Create();

        var consignee = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consigneeId)
            .Create();
        var consignor = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consignorId)
            .Create();
        var dispatchLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, dispatchLocationId)
            .Create();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { consignee, consignor },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { dispatchLocation }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().NotBeNull();
        result.Consignor.Should().NotBeNull();
        result.DestinationLocation.Should().BeNull();
        result.DispatchLocation.Should().NotBeNull();
    }

    [Fact]
    public void Map_ConvertFrom_Establishments_With_No_Matching_DispatchLocationId_Returns_No_DispatchLocation()
    {
        // Arrange
        var consigneeId = Guid.NewGuid();
        var consignorId = Guid.NewGuid();
        var destinationLocationId = Guid.NewGuid();

        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consigneeId.ToString() }])
                .Create())
            .With(s => s.Consignor, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consignorId.ToString() }])
                .Create())
            .With(s => s.DestinationLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = destinationLocationId.ToString() }])
                .Create())
            .With(s => s.DispatchLocation, new Fixture()
                .Create<GcModels.LogisticsLocation>())
            .Create();

        var consignee = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consigneeId)
            .Create();
        var consignor = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consignorId)
            .Create();
        var destinationLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, destinationLocationId)
            .Create();
        var dispatchLocation = new Fixture()
            .Create<Logic.Models.Idcoms.Establishment>();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { consignee, consignor },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { destinationLocation, dispatchLocation }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().NotBeNull();
        result.Consignor.Should().NotBeNull();
        result.DestinationLocation.Should().NotBeNull();
        result.DispatchLocation.Should().BeNull();
    }

    [Fact]
    public void Map_ConvertFrom_Establishments_Where_DispatchLocationId_IsNull_Returns_No_DestinationLocation()
    {
        // Arrange
        var consigneeId = Guid.NewGuid();
        var consignorId = Guid.NewGuid();
        var destinationLocationId = Guid.NewGuid();

        var sc = new Fixture().Build<GcModels.SupplyChainConsignment>()
            .With(s => s.Consignee, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consigneeId.ToString() }])
                .Create())
            .With(s => s.Consignor, new Fixture()
                .Build<GcModels.TradeParty>()
                .With(t => t.Id, [new() { Content = consignorId.ToString() }])
                .Create())
            .With(s => s.DestinationLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = destinationLocationId.ToString() }])
                .Create())
            .With(s => s.DispatchLocation, new Fixture()
                .Build<GcModels.LogisticsLocation>()
                .With(t => t.Id, [new() { Content = null }])
                .Create())
            .Create();

        var consignee = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consigneeId)
            .Create();
        var consignor = new Fixture()
            .Build<Logic.Models.Idcoms.Organisation>()
            .With(c => c.DefraCustomerId, consignorId)
            .Create();
        var destinationLocation = new Fixture()
            .Build<Logic.Models.Idcoms.Establishment>()
            .With(c => c.EstablishmentId, destinationLocationId)
            .Create();

        var enrichmentData = new IdcomsGeneralCertificateEnrichment
        {
            GcId = "gcid",
            Applicant = new Fixture().Create<CustomerContact>(),
            Organisations = new List<Logic.Models.Idcoms.Organisation> { consignee, consignor },
            Establishments = new List<Logic.Models.Idcoms.Establishment> { destinationLocation }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<Logic.Models.Idcoms.IdcomsGeneralCertificateEnrichment, GcModels.SupplyChainConsignment>(enrichmentData, sc);

        // Assert
        result.Should().NotBeNull();

        result.Consignee.Should().NotBeNull();
        result.Consignor.Should().NotBeNull();
        result.DestinationLocation.Should().NotBeNull();
        result.DispatchLocation.Should().BeNull();
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
            cfg.AddProfile<IdcomsGeneralCertificateEnrichmentProfile>();
        });
    }
}