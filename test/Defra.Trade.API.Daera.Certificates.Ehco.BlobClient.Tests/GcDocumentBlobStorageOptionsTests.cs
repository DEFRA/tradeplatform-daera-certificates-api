// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Tests;

public class GcDocumentBlobStorageOptionsTests
{
    [Fact]
    public void SectionName_ShouldBe_ExpectedValue()
    {
        // Arrange
        var sut = new GcDocumentBlobStorageOptions();

        // Assert
        sut.SectionName.Should().Be("DaeraCertificatesExtApi:GcDocumentBlobStorage");
    }

    [Fact]
    public void SourceContainerName_DefaultShouldBe_ExpectedValue()
    {
        // Arrange
        var sut = new GcDocumentBlobStorageOptions();

        // Assert
        sut.GetContainer(nameof(StorageContainerType.Source)).Should().BeSameAs("application-forms");
    }
}
