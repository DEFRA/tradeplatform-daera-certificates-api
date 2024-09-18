// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Dtos.Enums;
using Defra.Trade.API.Daera.Certificates.V1.Examples;

namespace Defra.Trade.API.Daera.Certificates.Tests.V1.Examples;

public class GeneralCertificateSummariesExampleTests
{
    [Fact]
    public void GetExamples_Valid_OK()
    {
        var itemUnderTest = new GeneralCertificateSummariesExample();

        var result = itemUnderTest.GetExamples();

        result.Should().NotBeNull();
        result.Data.First().GeneralCertificateId.Should().Be("GC-12001");
        result.Data.First().Status.Should().Be(CertificateStatus.Complete);
        result.Data.First().CreatedOn.Should().Be(new DateTimeOffset(2022, 12, 12, 12, 12, 12, TimeSpan.Zero));
        result.Data.First().LastUpdated.Should().Be(new DateTimeOffset(2023, 12, 12, 12, 12, 12, TimeSpan.Zero));
        result.Data.Count().Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.Records.Should().Be(2);
        result.TotalPages.Should().Be(1);
        result.TotalRecords.Should().Be(2);

        result.Data.First().Links[0].Href.Should().BeEquivalentTo("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate-summary?gcId=GC-12001");
        result.Data.First().Links[0].Rel.Should().BeEquivalentTo("general-certificate-summary-getbyid");
        result.Data.First().Links[0].Method.Should().BeEquivalentTo("GET");
        result.Data.First().Links[0].Description.Should().BeEquivalentTo("Endpoint to get a summary of a General Certificate by Id.");

        result.Data.First().Links[1].Href.Should().BeEquivalentTo("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate?gcId=GC-12001");
        result.Data.First().Links[1].Rel.Should().BeEquivalentTo("general-certificate-getbyid");
        result.Data.First().Links[1].Method.Should().BeEquivalentTo("GET");
        result.Data.First().Links[1].Description.Should().BeEquivalentTo("Endpoint to get the detailed payload of a General Certificate by Id.");

        result.Data.First().Documents.First().Links.Count.Should().Be(1);
        result.Data.First().Documents.First().Links.First().Href.Should().BeEquivalentTo("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate/document?gcId=GC-12001&documentId=GC-12001-doc1");
        result.Data.First().Documents.First().Links.First().Rel.Should().BeEquivalentTo("general-certificate-document-getbyid");
        result.Data.First().Documents.First().Links.First().Method.Should().BeEquivalentTo("GET");
        result.Data.First().Documents.First().Links.First().Description.Should().BeEquivalentTo("Endpoint to get a document from a General Certificate by GC Id and document Id.");
        result.Data.First().Documents.First().Id.Should().BeEquivalentTo("GC-12001-doc1");
        result.Data.First().Documents.First().TypeCode.Should().BeEquivalentTo("tc1");
    }
}