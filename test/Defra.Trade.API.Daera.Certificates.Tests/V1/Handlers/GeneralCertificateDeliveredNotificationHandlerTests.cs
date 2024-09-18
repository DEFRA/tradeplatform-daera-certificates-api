// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Threading;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.V1.Handlers;
using Defra.Trade.API.Daera.Certificates.V1.Notifications;

namespace Defra.Trade.API.Daera.Certificates.Tests.V1.Handlers;

public class GeneralCertificateDeliveredNotificationHandlerTests
{
    private readonly GeneralCertificateDeliveredNotificationHandler _sut;
    private readonly Mock<IGeneralCertificateCompletionService> _completionService;

    public GeneralCertificateDeliveredNotificationHandlerTests()
    {
        _completionService = new(MockBehavior.Strict);
        _sut = new(_completionService.Object);
    }

    [Fact]
    public async Task Handle_CallsTheCompletionService()
    {
        // arrange
        string id = Guid.NewGuid().ToString();
        var notification = new GeneralCertificateDeliveredNotification(id);
        using var cts = new CancellationTokenSource();

        _completionService.Setup(m => m.GeneralCertificateDelivered(id, cts.Token)).Returns(Task.CompletedTask).Verifiable();

        // act
        await _sut.Handle(notification, cts.Token);

        // assert
        Mock.Verify(_completionService);
    }
}
