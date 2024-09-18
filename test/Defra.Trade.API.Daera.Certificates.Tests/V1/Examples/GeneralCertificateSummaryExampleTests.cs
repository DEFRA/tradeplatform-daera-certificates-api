// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Dtos.Enums;
using Defra.Trade.API.Daera.Certificates.V1.Examples;

namespace Defra.Trade.API.Daera.Certificates.Tests.V1.Examples;

public class GeneralCertificateSummaryExampleTests
{
    [Fact]
    public void GetExamples_Valid_OK()
    {
        var itemUnderTest = new GeneralCertificateSummaryExample();

        var result = itemUnderTest.GetExamples();

        result.Should().NotBeNull();
        result.GeneralCertificateId.Should().Be("GC-12001");
        result.Status.Should().Be(CertificateStatus.Complete);
        result.CreatedOn.Should().Be(new DateTimeOffset(2022, 12, 12, 12, 12, 12, TimeSpan.Zero));
        result.LastUpdated.Should().Be(new DateTimeOffset(2023, 12, 12, 12, 12, 12, TimeSpan.Zero));
        result.Documents.Count.Should().Be(1);
        result.Links.Count.Should().Be(1);
        result.Links[0].Href.Should().BeEquivalentTo("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate?gcId=GC-12001");
        result.Links[0].Rel.Should().BeEquivalentTo("general-certificate-getbyid");
        result.Links[0].Method.Should().BeEquivalentTo("GET");
        result.Links[0].Description.Should().BeEquivalentTo("Endpoint to get the detailed payload of a General Certificate by Id.");
        result.Documents.First().Links.Count.Should().Be(1);
        result.Documents.First().Links.First().Href.Should().BeEquivalentTo("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate/document?gcId=GC-12001&documentId=GC-12001-doc1");
        result.Documents.First().Links.First().Rel.Should().BeEquivalentTo("general-certificate-document-getbyid");
        result.Documents.First().Links.First().Method.Should().BeEquivalentTo("GET");
        result.Documents.First().Links.First().Description.Should().BeEquivalentTo("Endpoint to get a document from a General Certificate by GC Id and document Id.");
        result.Documents.First().Id.Should().BeEquivalentTo("GC-12001-doc1");
        result.Documents.First().TypeCode.Should().BeEquivalentTo("tc1");
    }
}