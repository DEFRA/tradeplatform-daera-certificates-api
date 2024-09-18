// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Threading;
using Defra.Trade.API.Daera.Certificates.Extensions;
using Microsoft.AspNetCore.Http;

namespace Defra.Trade.API.Daera.Certificates.Tests.Extensions;

public static class HttpResponseExtensionsTests
{
    [Fact]
    public static async Task OnCompletedSuccessfully_CallsTheCallback_WhenTheResponseCompletesSuccessfully()
    {
        // arrange
        var response = new Mock<HttpResponse>(MockBehavior.Strict);
        var context = new Mock<HttpContext>(MockBehavior.Strict);
        var callback = new Mock<Func<Task>>(MockBehavior.Strict);
        var hook = null as Func<Task>;
        using var cts = new CancellationTokenSource();

        response.Setup(m => m.OnCompleted(It.IsAny<Func<Task>>()))
            .Callback((Func<Task> action) => { hook = action; })
            .Verifiable();

        // act
        response.Object.OnCompletedSuccessfully(callback.Object);

        // assert
        Mock.Verify(response, context, callback);
        hook.Should().NotBeNull();
        callback.Invocations.Should().BeEmpty();

        response.Setup(m => m.HttpContext).Returns(context.Object).Verifiable();
        context.Setup(m => m.RequestAborted).Returns(cts.Token).Verifiable();
        callback.Setup(m => m()).Returns(Task.CompletedTask).Verifiable();

        await hook();
        Mock.Verify(response, context, callback);
    }

    [Fact]
    public static async Task OnCompletedSuccessfully_DoesNotCallTheCallback_WhenTheResponseIsAborted()
    {
        // arrange
        var response = new Mock<HttpResponse>(MockBehavior.Strict);
        var context = new Mock<HttpContext>(MockBehavior.Strict);
        var callback = new Mock<Func<Task>>(MockBehavior.Strict);
        var hook = null as Func<Task>;
        using var cts = new CancellationTokenSource();

        response.Setup(m => m.OnCompleted(It.IsAny<Func<Task>>()))
            .Callback((Func<Task> action) => { hook = action; })
            .Verifiable();

        // act
        response.Object.OnCompletedSuccessfully(callback.Object);

        // assert
        Mock.Verify(response, context, callback);
        hook.Should().NotBeNull();
        callback.Invocations.Should().BeEmpty();

        await cts.CancelAsync();
        response.Setup(m => m.HttpContext).Returns(context.Object).Verifiable();
        context.Setup(m => m.RequestAborted).Returns(cts.Token).Verifiable();

        await hook();
        Mock.Verify(response, context, callback);
    }
}
