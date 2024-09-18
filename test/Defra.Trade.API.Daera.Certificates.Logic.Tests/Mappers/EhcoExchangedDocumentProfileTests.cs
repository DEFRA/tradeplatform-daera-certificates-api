// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Linq;
using AutoMapper;
using Defra.Trade.API.Daera.Certificates.Logic.Mappers;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Mappers;

public class EhcoExchangedDocumentProfileTests
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
        var source = new EhcoModels.ExchangedDocument
        {
            ID = "UK/GB/E&W/2023/1002003004001",
            TypeCode = "1001",
            ApplicantAssignedID = "Test App 123",
            ApplicationSubmissionID = "1002003004001",
            CertificateIssueDateTime = new DateTimeOffset(2023, 6, 5, 3, 40, 35, 201, TimeSpan.Zero),
            ApplicationSubmissionDateTime = new DateTimeOffset(2023, 5, 4, 2, 39, 23, 102, TimeSpan.Zero),
            PackingListFileLocation = "https://ehco.blob.store/abc-123",
            CertificatePDFLocation = null,
            Applicant = new EhcoModels.Applicant
            {
                DefraCustomer = new EhcoModels.DefraCustomer
                {
                    OrgId = new Guid("1EA07858-3969-4110-A8E6-F8C6B155D93D"),
                    UserId = new Guid("360C0F7C-78F6-4E5D-886C-4F96096FB5F5")
                }
            }
        };

        var sut = CreateSut();

        // Act
        var result = sut.Map<GcModels.ExchangedDocument>(source);

        // Assert
        result.Should().NotBeNull();

        result.Id.Content.Should().Be("UK/GB/E&W/2023/1002003004001");
        result.Id.SchemeId.Should().Be("GC");
        result.TypeCode.Content.Should().Be("1001");
        result.IssueDateTime.Content.Should().Be("202306050340+00:00");
        result.IssueDateTime.Format.Should().Be("205");

        var doc = result.ReferenceDocument.Should()
            .HaveCount(1)
            .And.Subject.First();

        doc.TypeCode.Content.Should().Be("271");
        doc.TypeCode.ListAgencyID.Should().Be("6");

        doc.Id.Should().HaveCount(1)
            .And.Subject.First().Content.Should().Be("abc-123");

        var attachment = doc.AttachedBinaryObject.Should().HaveCount(1).And.Subject.First();
        attachment.Uri.Should().Be("https://ehco.blob.store/abc-123");
        attachment.Filename.Should().Be("abc-123");

        result.PrimarySignatoryAuthentication.ActualDateTime.Content.Should().Be("202305040239+00:00");

        var clause = result.PrimarySignatoryAuthentication.IncludedDocumentClause.Should().HaveCount(1)
            .And.Subject.First();
        clause.Id.Content.Should().Be("GC Declaration Clause 873");
        clause.Id.SchemeId.Should().Be("DC");
        clause.Content.Content.Should().Be("I, the undersigned operator responsible for the consignment detailed " +
            "above, certify that to the best of my knowledge and belief the statements made in Part I of this " +
            "document are true and complete, and I agree to comply with the requirements of Regulation (EU) " +
            "2017/625 on official controls, including payment for official controls, as well as for re-dispatching " +
            "consignments, quarantine or isolation of animals, or costs of euthanasia and disposal where necessary.");
        clause.Content.LanguageId.Should().Be("en");

        var providingPartyId = result.PrimarySignatoryAuthentication.ProvidingTradeParty.Id.Should().HaveCount(1)
            .And.Subject.First();
        providingPartyId.Content.Should().Be("1ea07858-3969-4110-a8e6-f8c6b155d93d");
        result.PrimarySignatoryAuthentication.ProvidingTradeParty.RoleCode.Should().HaveCount(1).And.Subject.First()
            .Content.Should().Be("DGP");

        result.Issuer.Name.Should().HaveCount(1).And.Subject.First().Content.Should()
            .Be("Department for Environment Food & Rural Affairs");
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
            cfg.AddProfile<EhcoExchangedDocumentProfile>();
        });
    }
}