// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Examples;

namespace Defra.Trade.API.Daera.Certificates.Tests.V1.Examples;

public class MetadataExampleTests
{
    [Fact]
    public void MetadataExample_GetExample_ShouldHaveValidExample()
    {
        var itemUnderTest = new MetadataExample();

        var result = itemUnderTest.GetExamples();

        result.Should().NotBeNull();
        result.Links.Count.Should().Be(4);

        result.Links[0].Href.Should().BeEquivalentTo("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate-summaries");
        result.Links[0].Rel.Should().BeEquivalentTo("general-certificate-summaries");
        result.Links[0].Method.Should().BeEquivalentTo("GET");
        result.Links[0].Description.Should().BeEquivalentTo("Endpoint to get a filtered and paginated list of General Certificate summaries.");

        result.Links[1].Href.Should().BeEquivalentTo("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate-summary?gcId={gcId}");
        result.Links[1].Rel.Should().BeEquivalentTo("general-certificate-summary-getbyid");
        result.Links[1].Method.Should().BeEquivalentTo("GET");
        result.Links[1].Description.Should().BeEquivalentTo("Endpoint to get a summary of a General Certificate by Id.");

        result.Links[2].Href.Should().BeEquivalentTo("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate?gcId={gcId}");
        result.Links[2].Rel.Should().BeEquivalentTo("general-certificate-getbyid");
        result.Links[2].Method.Should().BeEquivalentTo("GET");
        result.Links[2].Description.Should().BeEquivalentTo("Endpoint to get the detailed payload of a General Certificate by Id.");

        result.Links[3].Href.Should().BeEquivalentTo("https://gateway.trade.defra.gov.uk/daera-certificates/v1/general-certificate/document?gcId={gcId}&documentId={documentId}");
        result.Links[3].Rel.Should().BeEquivalentTo("general-certificate-document-getbyid");
        result.Links[3].Method.Should().BeEquivalentTo("GET");
        result.Links[3].Description.Should().BeEquivalentTo("Endpoint to get a document from a General Certificate by GC Id and document Id.");
    }
}