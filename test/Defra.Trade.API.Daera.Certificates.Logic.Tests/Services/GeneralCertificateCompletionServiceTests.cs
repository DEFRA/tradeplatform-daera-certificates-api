// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Threading;
using Defra.Trade.API.CertificatesStore.V1.ApiClient.Api;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Logic.Services;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Repository.Interfaces;
using Defra.Trade.API.Daera.Certificates.Tests.Common;
using Microsoft.Extensions.Logging;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Services;

public class GeneralCertificateCompletionServiceTests
{
    private readonly Mock<IDocumentRetrievalApi> _documentRetrievalApi;
    private readonly Mock<ICertificatesStoreRepository> _generalCertificatesRepository;
    private readonly Mock<ILogger<GeneralCertificateCompletionService>> _logger;
    private readonly Mock<IEhcoNotificationService> _notificationService;
    private readonly GeneralCertificateCompletionService _sut;

    public GeneralCertificateCompletionServiceTests()
    {
        _notificationService = new(MockBehavior.Strict);
        _generalCertificatesRepository = new(MockBehavior.Strict);
        _documentRetrievalApi = new();
        _logger = new();
        _logger.Setup(m => m.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        _sut = new(
            _notificationService.Object,
            _generalCertificatesRepository.Object,
            _documentRetrievalApi.Object,
            _logger.Object);
    }

    [Fact]
    public void GeneralCertificateDelivered_DoesNothing()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        string id = Guid.NewGuid().ToString();

        // act
        var actual = _sut.GeneralCertificateDelivered(id, cts.Token);

        // assert
        actual.Should().BeSameAs(Task.CompletedTask);
        Mock.Verify(_notificationService);
    }

    [Fact]
    public async Task V1DocumentDelivered_NotifiesDaera()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        string gcId = Guid.NewGuid().ToString();
        string documentId = Guid.NewGuid().ToString();
        var packingListId = Guid.NewGuid();
        var gc = new GeneralCertificate
        {
            SchemaVersion = 1,
            GeneralCertificateDocument =
            [
                new()
                {
                    Id = packingListId,
                    TypeCode = 271,
                    Retrieved = DateTime.UtcNow.AddSeconds(-3)
                }
            ]
        };

        _notificationService
            .Setup(m => m.NotifyGeneralCertificateDelivered(gcId, cts.Token))
            .Returns(Task.CompletedTask);

        _generalCertificatesRepository
            .Setup(m => m.GetGeneralCertificateWithDocumentsAsync(gcId, cts.Token))
            .ReturnsAsync(gc)
            .Verifiable();

        _documentRetrievalApi
            .Setup(m => m.GetDocumentRetrievedAsync(packingListId, It.IsAny<string>(), It.IsAny<int>(), cts.Token))
            .ReturnsAsync(true)
            .Verifiable();

        // act
        await _sut.DocumentDelivered(gcId, documentId, cts.Token);

        // assert
        Mock.Verify(_notificationService);
    }

    [Fact]
    public async Task V2DocumentDelivered_NotifiesDaera()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        string gcId = Guid.NewGuid().ToString();
        string documentId = Guid.NewGuid().ToString();
        var packingListId = Guid.NewGuid();
        var certPfdId = Guid.NewGuid();
        var gc = new GeneralCertificate
        {
            SchemaVersion = 2,
            GeneralCertificateDocument =
            [
                new()
                {
                    Id = packingListId,
                    TypeCode = 271,
                    Retrieved = DateTime.UtcNow.AddSeconds(-3)
                },
                new()
                {
                    Id = certPfdId,
                    TypeCode = 16,
                    Retrieved = DateTime.UtcNow.AddSeconds(-10)
                }
            ]
        };

        _notificationService
            .Setup(m => m.NotifyGeneralCertificateDelivered(gcId, cts.Token))
            .Returns(Task.CompletedTask);

        _generalCertificatesRepository
            .Setup(m => m.GetGeneralCertificateWithDocumentsAsync(gcId, cts.Token))
            .ReturnsAsync(gc)
            .Verifiable();

        _documentRetrievalApi
            .Setup(m => m.GetDocumentRetrievedAsync(packingListId, It.IsAny<string>(), It.IsAny<int>(), cts.Token))
            .ReturnsAsync(true)
            .Verifiable();

        _documentRetrievalApi
            .Setup(m => m.GetDocumentRetrievedAsync(certPfdId, It.IsAny<string>(), It.IsAny<int>(), cts.Token))
            .ReturnsAsync(true)
            .Verifiable();

        // act
        await _sut.DocumentDelivered(gcId, documentId, cts.Token);

        // assert
        Mock.Verify(_notificationService);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public async Task V2NotAllDocumentsDelivered_DefersNotificationToDaera(bool p, bool c)
    {
        // arrange
        using var cts = new CancellationTokenSource();
        string gcId = Guid.NewGuid().ToString();
        string documentId = Guid.NewGuid().ToString();
        var packingListId = Guid.NewGuid();
        var certPfdId = Guid.NewGuid();
        var gc = new GeneralCertificate
        {
            GeneralCertificateId = gcId,
            SchemaVersion = 2,
            GeneralCertificateDocument =
            [
                new()
                {
                    Id = packingListId,
                    TypeCode = 271,
                    Retrieved = DateTime.UtcNow.AddSeconds(-3)
                },
                new()
                {
                    Id = certPfdId,
                    TypeCode = 16,
                    Retrieved = null
                }
            ]
        };

        _notificationService.Setup(m => m.NotifyGeneralCertificateDelivered(gcId, cts.Token))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _generalCertificatesRepository.Setup(m => m.GetGeneralCertificateWithDocumentsAsync(gcId, cts.Token))
            .ReturnsAsync(gc)
            .Verifiable();

        _documentRetrievalApi
            .Setup(m => m.GetDocumentRetrievedAsync(packingListId, It.IsAny<string>(), It.IsAny<int>(), cts.Token))
            .ReturnsAsync(p)
            .Verifiable();

        _documentRetrievalApi
            .Setup(m => m.GetDocumentRetrievedAsync(certPfdId, It.IsAny<string>(), It.IsAny<int>(), cts.Token))
            .ReturnsAsync(c)
            .Verifiable();

        // act
        await _sut.DocumentDelivered(gcId, documentId, cts.Token);

        // assert
        _logger.VerifyLogged($"All documents not yet retrieved by DAERA for {gcId}, notification to EHCO deferred.", LogLevel.Information);
    }
}
