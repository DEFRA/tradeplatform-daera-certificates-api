// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Examples;

namespace Defra.Trade.API.Daera.Certificates.Tests.V1.Examples;

public class GeneralCertificateExampleTests
{
    [Fact]
    public void GetExamples_Valid_OK()
    {
        var itemUnderTest = new GeneralCertificateExample();

        var result = itemUnderTest.GetExamples();

        result.Should().NotBeNull();
        result.ExchangedDocument.Id.Content.Should().Be("CHEDP.XI.2023.0061020");
        result.ExchangedDocument.Id.SchemeId.Should().Be("GC");
    }
}