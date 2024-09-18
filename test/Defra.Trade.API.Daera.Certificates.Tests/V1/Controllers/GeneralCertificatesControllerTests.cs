// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Threading;
using Defra.Trade.API.Daera.Certificates.V1.Controllers;
using Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;
using Defra.Trade.API.Daera.Certificates.V1.Notifications;
using Defra.Trade.API.Daera.Certificates.V1.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;

namespace Defra.Trade.API.Daera.Certificates.Tests.V1.Controllers;

public class GeneralCertificatesControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly GeneralCertificatesController _sut;
    private readonly Mock<ILogger<GeneralCertificatesController>> _logger;

    public GeneralCertificatesControllerTests()
    {
        _mediator = new(MockBehavior.Strict);
        _logger = new();
        _sut = new(_mediator.Object, _logger.Object);
    }

    [Fact]
    public async Task GetGeneralCertificateDocumentById_ReturnsNotFound_WhenTheQueryReturnsNull()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        var query = new GetGeneralCertificateByIdQuery();
        var result = null as GeneralCertificate;

        _mediator.Setup(m => m.Send(query, cts.Token)).ReturnsAsync(result).Verifiable();

        // act
        var actual = await _sut.GetGeneralCertificateById(query, cts.Token);

        // assert
        actual.Should().BeOfType<NotFoundResult>();
        Mock.Verify(_mediator);
    }

    [Fact]
    public async Task GetGeneralCertificateDocumentById_ReturnsFile_WhenTheQueryIsSuccessful()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        var query = new GetGeneralCertificateByIdQuery();
        var result = new GeneralCertificate();
        var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
        var response = new Mock<HttpResponse>(MockBehavior.Strict);
        var onCompleted = null as Func<Task>;
        var actionContext = new ActionContext(httpContext.Object, new(), new ControllerActionDescriptor());

        _sut.ControllerContext = new(actionContext);
        httpContext.Setup(m => m.Response).Returns(response.Object).Verifiable();
        response.Setup(m => m.OnCompleted(It.IsAny<Func<Task>>()))
            .Callback((Func<Task> action) => onCompleted = action)
            .Verifiable();
        _mediator.Setup(m => m.Send(query, cts.Token)).ReturnsAsync(result).Verifiable();

        // act
        var actual = await _sut.GetGeneralCertificateById(query, cts.Token);

        // assert
        var fileStreamResult = actual.Should().BeOfType<OkObjectResult>().Subject;
        fileStreamResult.Value.Should().BeSameAs(result);
        onCompleted.Should().NotBeNull();
        Mock.Verify(_mediator, httpContext, response);

        response.Setup(m => m.HttpContext).Returns(httpContext.Object).Verifiable();
        httpContext.Setup(m => m.RequestAborted).Returns(cts.Token).Verifiable();
        _mediator.Setup(m => m.Publish(It.Is<GeneralCertificateDeliveredNotification>(n => n.GeneralCertificateId == query.GcId), default))
            .Returns(Task.CompletedTask)
            .Verifiable();
        await onCompleted();

        Mock.Verify(_mediator, httpContext, response);
    }
}
