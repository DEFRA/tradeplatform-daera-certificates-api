// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Extensions;

public static class HttpResponseExtensions
{
    public static void OnCompletedSuccessfully(this HttpResponse response, Func<Task> action)
    {
        response.OnCompleted(() =>
        {
            if (response.HttpContext.RequestAborted.IsCancellationRequested)
                return Task.CompletedTask;

            return action();
        });
    }
}