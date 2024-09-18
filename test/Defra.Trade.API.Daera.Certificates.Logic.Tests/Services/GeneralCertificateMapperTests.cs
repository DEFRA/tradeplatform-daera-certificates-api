// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoFixture;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Logic.Infrastructure;
using Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;
using Defra.Trade.API.Daera.Certificates.Logic.Services;
using Microsoft.Extensions.Options;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

using IdcomsModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Services;

public class GeneralCertificateMapperTests
{
    private readonly Mock<IOptions<ApimExternalApisSettings>> _loggerMock = new();
    private readonly GeneralCertificateMapper _sut;

    public GeneralCertificateMapperTests()
    {
        _loggerMock.Setup(x => x.Value).Returns(
            new ApimExternalApisSettings { BaseUrl = "mock-base", DaeraCertificatesApiPathV1 = "mock-api" });
        _sut = new GeneralCertificateMapper(_loggerMock.Object);
    }

    [Fact]
    public void ConsigneeAndConsignor_IsNullId_ShouldReturnNull()
    {
        // Arrange
        var fixture = new Fixture();
        var ehcoGeneralCertificateApplication = fixture.Build<EhcoModels.EhcoGeneralCertificateApplication>()
            .Create();
        ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId = Guid.Empty;
        ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId = Guid.Empty;

        var applicantOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.OrgId).Create();
        var consigneeOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId).Create();

        var consignorOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var organisations = new List<Organisation> { consigneeOrgId, consignorOrgId, applicantOrgId };

        var establishmentOne = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishmentTwo = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishments = new List<Establishment> { establishmentTwo, establishmentOne };
        var enrichedSourceData = fixture.Build<IdcomsModels.IdcomsGeneralCertificateEnrichment>()
            .With(x => x.Applicant, fixture.Build<CustomerContact>()
                .With(x => x.DefraCustomerId, ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.UserId)
                .Create())
            .With(x => x.Organisations, organisations)
            .With(x => x.Establishments, establishments)
            .Create();

        var data = fixture.Build<Database.Models.GeneralCertificate>()
            .With(x => x.Data, JsonSerializer.Serialize(ehcoGeneralCertificateApplication, GetSerializerOptions()))
            .Without(x => x.GeneralCertificateDocument)
            .With(x => x.EnrichmentData, fixture.Build<Database.Models.EnrichmentData>()
                .With(x => x.Data, JsonSerializer.Serialize(enrichedSourceData, GetSerializerOptions()))
                .Without(x => x.GeneralCertificate)
                .Create())
            .Create();
        var result = _sut.Map(data);

        result.SupplyChainConsignment.Consignor.Should().BeNull();
        result.SupplyChainConsignment.Consignee.Should().BeNull();
    }

    [Fact]
    public void Applicant_IsNullId_ShouldReturnNull()
    {
        // Arrange
        var fixture = new Fixture();
        var ehcoGeneralCertificateApplication = fixture.Build<EhcoModels.EhcoGeneralCertificateApplication>()
            .Create();
        ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId = Guid.Empty;
        ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId = Guid.Empty;

        var consigneeOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId).Create();

        var consignorOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var organisations = new List<Organisation> { consigneeOrgId, consignorOrgId };

        var establishmentOne = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishmentTwo = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishments = new List<Establishment> { establishmentTwo, establishmentOne };
        var enrichedSourceData = fixture.Build<IdcomsModels.IdcomsGeneralCertificateEnrichment>()
            .With(x => x.Applicant, fixture.Build<CustomerContact>()
                .With(x => x.DefraCustomerId, ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.UserId)
                .Create())
            .With(x => x.Organisations, organisations)
            .With(x => x.Establishments, establishments)
            .Create();

        var data = fixture.Build<Database.Models.GeneralCertificate>()
            .With(x => x.Data, JsonSerializer.Serialize(ehcoGeneralCertificateApplication, GetSerializerOptions()))
            .Without(x => x.GeneralCertificateDocument)
            .With(x => x.EnrichmentData, fixture.Build<Database.Models.EnrichmentData>()
                .With(x => x.Data, JsonSerializer.Serialize(enrichedSourceData, GetSerializerOptions()))
                .Without(x => x.GeneralCertificate)
                .Create())
            .Create();
        var result = _sut.Map(data);

        result.ExchangedDocument.Issuer.AuthoritativeSignatoryPerson.Should().BeNull();
    }

    [Fact]
    public void WhenCountryIs_Null_ShouldReturnNull()
    {
        // Arrange
        var fixture = new Fixture();
        var ehcoGeneralCertificateApplication = fixture.Build<EhcoModels.EhcoGeneralCertificateApplication>()
            .Create();
        ehcoGeneralCertificateApplication.SupplyChainConsignment.ExportCountry = string.Empty;
        var applicantOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.OrgId).Create();
        var consigneeOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId).Create();

        var consignorOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var organisations = new List<Organisation> { consigneeOrgId, consignorOrgId, applicantOrgId };

        var establishmentOne = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishmentTwo = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishments = new List<Establishment> { establishmentTwo, establishmentOne };
        var enrichedSourceData = fixture.Build<IdcomsModels.IdcomsGeneralCertificateEnrichment>()
            .With(x => x.Applicant, fixture.Build<CustomerContact>()
                .With(x => x.DefraCustomerId, ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.UserId)
                .Create())
            .With(x => x.Organisations, organisations)
            .With(x => x.Establishments, establishments)
            .Create();

        var data = fixture.Build<Database.Models.GeneralCertificate>()
            .With(x => x.Data, JsonSerializer.Serialize(ehcoGeneralCertificateApplication, GetSerializerOptions()))
            .Without(x => x.GeneralCertificateDocument)
            .With(x => x.EnrichmentData, fixture.Build<Database.Models.EnrichmentData>()
                .With(x => x.Data, JsonSerializer.Serialize(enrichedSourceData, GetSerializerOptions()))
                .Without(x => x.GeneralCertificate)
                .Create())
            .Create();

        var referenceDocuments = fixture.Build<GeneralCertificateDocument>()
            .With(x => x.GeneralCertificate, data)
            .CreateMany(2);

        data.GeneralCertificateDocument = referenceDocuments.ToList();
        // Act
        var result = _sut.Map(data);

        result.SupplyChainConsignment.ExportCountry.Should().BeNull();
    }

    [Fact]
    public void MapReferenceDocument_WhenNull_ShouldReturnNull()
    {
        // Arrange
        var fixture = new Fixture();
        var ehcoGeneralCertificateApplication = fixture.Build<EhcoModels.EhcoGeneralCertificateApplication>()
            .Create();

        ehcoGeneralCertificateApplication.ExchangedDocument.PackingListFileLocation = string.Empty;
        var applicantOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.OrgId).Create();
        var consigneeOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId).Create();

        var consignorOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var organisations = new List<Organisation> { consigneeOrgId, consignorOrgId, applicantOrgId };

        var establishmentOne = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishmentTwo = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishments = new List<Establishment> { establishmentTwo, establishmentOne };
        var enrichedSourceData = fixture.Build<IdcomsModels.IdcomsGeneralCertificateEnrichment>()
            .With(x => x.Applicant, fixture.Build<CustomerContact>()
                .With(x => x.DefraCustomerId, ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.UserId)
                .Create())
            .With(x => x.Organisations, organisations)
            .With(x => x.Establishments, establishments)
            .Create();

        var data = fixture.Build<Database.Models.GeneralCertificate>()
            .With(x => x.Data, JsonSerializer.Serialize(ehcoGeneralCertificateApplication, GetSerializerOptions()))
            .Without(x => x.GeneralCertificateDocument)
            .With(x => x.EnrichmentData, fixture.Build<Database.Models.EnrichmentData>()
                .With(x => x.Data, JsonSerializer.Serialize(enrichedSourceData, GetSerializerOptions()))
                .Without(x => x.GeneralCertificate)
                .Create())
            .Create();

        // Act
        var result = _sut.Map(data);

        result.ExchangedDocument.ReferenceDocument.Should().BeEmpty();
    }

    [Fact]
    public void MapTemperature_WhenNull_ShouldReturnNull()
    {
        // Arrange
        var fixture = new Fixture();
        var ehcoGeneralCertificateApplication = fixture.Build<EhcoModels.EhcoGeneralCertificateApplication>()
            .Create();

        ehcoGeneralCertificateApplication.SupplyChainConsignment.UsedTransportEquipment.TemperatureSetting = string.Empty;
        var applicantOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.OrgId).Create();
        var consigneeOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId).Create();

        var consignorOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var organisations = new List<Organisation> { consigneeOrgId, consignorOrgId, applicantOrgId };

        var establishmentOne = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishmentTwo = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishments = new List<Establishment> { establishmentTwo, establishmentOne };
        var enrichedSourceData = fixture.Build<IdcomsModels.IdcomsGeneralCertificateEnrichment>()
            .With(x => x.Applicant, fixture.Build<CustomerContact>()
                .With(x => x.DefraCustomerId, ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.UserId)
                .Create())
            .With(x => x.Organisations, organisations)
            .With(x => x.Establishments, establishments)
            .Create();
        var data = fixture.Build<Database.Models.GeneralCertificate>()
            .With(x => x.Data, JsonSerializer.Serialize(ehcoGeneralCertificateApplication, GetSerializerOptions()))
            .Without(x => x.GeneralCertificateDocument)
            .With(x => x.EnrichmentData, fixture.Build<Database.Models.EnrichmentData>()
                .With(x => x.Data, JsonSerializer.Serialize(enrichedSourceData, GetSerializerOptions()))
                .Without(x => x.GeneralCertificate)
                .Create())
            .Create();
        var referenceDocuments = fixture.Build<GeneralCertificateDocument>()
            .With(x => x.GeneralCertificate, data)
            .CreateMany(2);

        data.GeneralCertificateDocument = referenceDocuments.ToList();

        // Act
        var result = _sut.Map(data);

        result.SupplyChainConsignment.UsedTransportEquipment.FirstOrDefault().TemperatureSetting.Should().BeEmpty();
    }

    [Fact]
    public void MapTemperature_WhenInvalid_ShouldReturnNull()
    {
        // Arrange
        var fixture = new Fixture();
        var ehcoGeneralCertificateApplication = fixture.Build<EhcoModels.EhcoGeneralCertificateApplication>()
            .Create();

        ehcoGeneralCertificateApplication.SupplyChainConsignment.UsedTransportEquipment.TemperatureSetting = "invalid";
        var applicantOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.OrgId).Create();
        var consigneeOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId).Create();

        var consignorOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var organisations = new List<Organisation> { consigneeOrgId, consignorOrgId, applicantOrgId };

        var establishmentOne = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishmentTwo = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishments = new List<Establishment> { establishmentTwo, establishmentOne };
        var enrichedSourceData = fixture.Build<IdcomsModels.IdcomsGeneralCertificateEnrichment>()
            .With(x => x.Applicant, fixture.Build<CustomerContact>()
                .With(x => x.DefraCustomerId, ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.UserId)
                .Create())
            .With(x => x.Organisations, organisations)
            .With(x => x.Establishments, establishments)
            .Create();

        var data = fixture.Build<Database.Models.GeneralCertificate>()
            .With(x => x.Data, JsonSerializer.Serialize(ehcoGeneralCertificateApplication, GetSerializerOptions()))
            .Without(x => x.GeneralCertificateDocument)
            .With(x => x.EnrichmentData, fixture.Build<Database.Models.EnrichmentData>()
                .With(x => x.Data, JsonSerializer.Serialize(enrichedSourceData, GetSerializerOptions()))
                .Without(x => x.GeneralCertificate)
                .Create())
            .Create();
        var referenceDocuments = fixture.Build<GeneralCertificateDocument>()
            .With(x => x.GeneralCertificate, data)
            .CreateMany(2);

        data.GeneralCertificateDocument = referenceDocuments.ToList();

        // Act
        var result = _sut.Map(data);

        result.SupplyChainConsignment.UsedTransportEquipment.FirstOrDefault().TemperatureSetting.Should().BeEmpty();
    }

    [Fact]
    public void GeneralCertificateMapper_WhenCalled_ShouldMapAsExpected()
    {
        // Arrange
        var fixture = new Fixture();
        var ehcoGeneralCertificateApplication = fixture
            .Build<EhcoModels.EhcoGeneralCertificateApplication>()
            .Create();

        var applicantOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.OrgId).Create();

        var consigneeOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId).Create();

        var consignorOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var organisations = new List<Organisation> { applicantOrgId, consigneeOrgId, consignorOrgId };

        var establishmentOne = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishmentTwo = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishments = new List<Establishment> { establishmentTwo, establishmentOne };
        var applicantEnriched = fixture.Build<CustomerContact>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.UserId).Create();
        var enrichedSourceData = fixture.Build<IdcomsModels.IdcomsGeneralCertificateEnrichment>()
            .With(x => x.Applicant, applicantEnriched)
            .With(x => x.Organisations, organisations)
            .With(x => x.Establishments, establishments)
            .Create();

        var data = fixture.Build<Database.Models.GeneralCertificate>()
            .With(x => x.Data, JsonSerializer.Serialize(ehcoGeneralCertificateApplication, GetSerializerOptions()))
            .Without(x => x.GeneralCertificateDocument)
            .With(x => x.EnrichmentData, fixture.Build<Database.Models.EnrichmentData>()
                .With(x => x.Data, JsonSerializer.Serialize(enrichedSourceData, GetSerializerOptions()))
                .Without(x => x.GeneralCertificate)
                .Create())
            .Create();

        var referenceDocuments = fixture.Build<GeneralCertificateDocument>()
            .With(x => x.GeneralCertificate, data)
            .CreateMany(2);

        data.GeneralCertificateDocument = referenceDocuments.ToList();

        // Act
        var result = _sut.Map(data);

        // Assert
        result.ExchangedDocument.PrimarySignatoryAuthentication.ProvidingTradeParty.AuthoritativeSignatoryPerson.Should().NotBeNull();
        result.ExchangedDocument.PrimarySignatoryAuthentication.ProvidingTradeParty.Name[0].Content.Should().BeEquivalentTo(applicantOrgId.Name);
        result.ExchangedDocument.PrimarySignatoryAuthentication.ProvidingTradeParty.AuthoritativeSignatoryPerson.Name.Content.Should()
            .BeEquivalentTo(applicantEnriched.Name);

        result.SupplyChainConsignment.ExportCountry.Code.Content.Should().Be(ehcoGeneralCertificateApplication.SupplyChainConsignment.ExportCountry);
        result.SupplyChainConsignment.ImportCountry.Code.Content.Should().Be(ehcoGeneralCertificateApplication.SupplyChainConsignment.ImportCountry);
    }

    [Fact]
    public void GeneralCertificateMapperWithOutTraces_WhenCalled_ShouldMapAsExpectedWithNullTraces()
    {
        // Arrange
        var fixture = new Fixture();
        var ehcoGeneralCertificateApplication = fixture
            .Build<EhcoModels.EhcoGeneralCertificateApplication>()
            .Create();
        ehcoGeneralCertificateApplication.SupplyChainConsignment.OperatorResponsibleForConsignment.Traces.OperatorId = null;
        var applicantOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.OrgId).Create();

        var consigneeOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignee.DefraCustomer.OrgId).Create();

        var consignorOrgId = fixture.Build<Organisation>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var organisations = new List<Organisation> { applicantOrgId, consigneeOrgId, consignorOrgId };

        var establishmentOne = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishmentTwo = fixture.Build<Establishment>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.SupplyChainConsignment.Consignor.DefraCustomer.OrgId).Create();

        var establishments = new List<Establishment> { establishmentTwo, establishmentOne };
        var applicantEnriched = fixture.Build<CustomerContact>().With(
            x => x.DefraCustomerId,
            ehcoGeneralCertificateApplication.ExchangedDocument.Applicant.DefraCustomer.UserId).Create();
        var enrichedSourceData = fixture.Build<IdcomsModels.IdcomsGeneralCertificateEnrichment>()
            .With(x => x.Applicant, applicantEnriched)
            .With(x => x.Organisations, organisations)
            .With(x => x.Establishments, establishments)
            .Create();

        var data = fixture.Build<Database.Models.GeneralCertificate>()
            .With(x => x.Data, JsonSerializer.Serialize(ehcoGeneralCertificateApplication, GetSerializerOptions()))
            .Without(x => x.GeneralCertificateDocument)
            .With(x => x.EnrichmentData, fixture.Build<Database.Models.EnrichmentData>()
                .With(x => x.Data, JsonSerializer.Serialize(enrichedSourceData, GetSerializerOptions()))
                .Without(x => x.GeneralCertificate)
                .Create())
            .Create();

        var referenceDocuments = fixture.Build<GeneralCertificateDocument>()
            .With(x => x.GeneralCertificate, data)
            .CreateMany(2);

        data.GeneralCertificateDocument = referenceDocuments.ToList();

        // Act
        var result = _sut.Map(data);

        // Assert
        result.SupplyChainConsignment.OperatorResponsibleForConsignment.Id.FirstOrDefault().Content.Should().BeNull();
    }

    internal static JsonSerializerOptions GetSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        options.Converters.Add(new JsonStringEnumConverter());

        return options;
    }
}
