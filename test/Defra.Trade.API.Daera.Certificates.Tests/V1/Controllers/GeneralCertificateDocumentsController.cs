// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.IO;
using System.Threading;
using Defra.Trade.API.Daera.Certificates.V1.Controllers;
using Defra.Trade.API.Daera.Certificates.V1.Dtos;
using Defra.Trade.API.Daera.Certificates.V1.Notifications;
using Defra.Trade.API.Daera.Certificates.V1.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;

namespace Defra.Trade.API.Daera.Certificates.Tests.V1.Controllers;

public class GeneralCertificateDocumentsControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly GeneralCertificateDocumentsController _sut;
    private readonly Mock<ILogger<GeneralCertificateDocumentsController>> _logger;

    public GeneralCertificateDocumentsControllerTests()
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
        var query = new GetGeneralCertificateDocumentByIdQuery();
        var result = null as ApplicationAttachments;

        _mediator.Setup(m => m.Send(query, cts.Token)).ReturnsAsync(result).Verifiable();

        // act
        var actual = await _sut.GetGeneralCertificateDocumentById(query, cts.Token);

        // assert
        actual.Should().BeOfType<NotFoundResult>();
        Mock.Verify(_mediator);
    }

    [Fact]
    public async Task GetGeneralCertificateDocumentById_ReturnsNotFound_WhenTheQueryReturnsNoFileContext()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        var query = new GetGeneralCertificateDocumentByIdQuery();
        var result = new ApplicationAttachments();

        _mediator.Setup(m => m.Send(query, cts.Token)).ReturnsAsync(result).Verifiable();

        // act
        var actual = await _sut.GetGeneralCertificateDocumentById(query, cts.Token);

        // assert
        actual.Should().BeOfType<NotFoundResult>();
        Mock.Verify(_mediator);
    }

    [Fact]
    public async Task GetGeneralCertificateDocumentById_ReturnsNotFound_WhenTheQueryReturnsNoContentType()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        var query = new GetGeneralCertificateDocumentByIdQuery();
        var result = new ApplicationAttachments
        {
            FileContext = new()
            {
                FileContent = new MemoryStream()
            }
        };

        _mediator.Setup(m => m.Send(query, cts.Token)).ReturnsAsync(result).Verifiable();

        // act
        var actual = await _sut.GetGeneralCertificateDocumentById(query, cts.Token);

        // assert
        actual.Should().BeOfType<NotFoundResult>();
        Mock.Verify(_mediator);
    }

    [Fact]
    public async Task GetGeneralCertificateDocumentById_ReturnsNotFound_WhenTheQueryReturnsNoContent()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        var query = new GetGeneralCertificateDocumentByIdQuery();
        var result = new ApplicationAttachments
        {
            FileContext = new()
            {
                ContentType = "application/octet-stream"
            }
        };

        _mediator.Setup(m => m.Send(query, cts.Token)).ReturnsAsync(result).Verifiable();

        // act
        var actual = await _sut.GetGeneralCertificateDocumentById(query, cts.Token);

        // assert
        actual.Should().BeOfType<NotFoundResult>();
        Mock.Verify(_mediator);
    }

    [Fact]
    public async Task GetGeneralCertificateDocumentById_ReturnsFile_WhenTheQueryIsSuccessful()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        var query = new GetGeneralCertificateDocumentByIdQuery
        {
            DocumentId = Guid.NewGuid().ToString(),
            GcId = Guid.NewGuid().ToString()
        };
        var fileContent = new MemoryStream();
        var result = new ApplicationAttachments
        {
            FileContext = new()
            {
                ContentType = "application/octet-stream",
                FileContent = fileContent
            }
        };
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
        var actual = await _sut.GetGeneralCertificateDocumentById(query, cts.Token);

        // assert
        var fileStreamResult = actual.Should().BeOfType<FileStreamResult>().Subject;
        fileStreamResult.ContentType.Should().Be("application/octet-stream");
        fileStreamResult.FileStream.Should().BeSameAs(fileContent);
        onCompleted.Should().NotBeNull();
        Mock.Verify(_mediator, httpContext, response);

        response.Setup(m => m.HttpContext).Returns(httpContext.Object).Verifiable();
        httpContext.Setup(m => m.RequestAborted).Returns(cts.Token).Verifiable();
        _mediator.Setup(m => m.Publish(It.Is<GeneralCertificateDocumentDeliveredNotification>(n => n.DocumentId == query.DocumentId && n.GeneralCertificateId == query.GcId), default))
            .Returns(Task.CompletedTask)
            .Verifiable();
        await onCompleted();

        Mock.Verify(_mediator, httpContext, response);
    }
}
