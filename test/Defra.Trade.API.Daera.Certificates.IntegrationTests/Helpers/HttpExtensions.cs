// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.IntegrationTests.Helpers;

public static class HttpExtensions
{
    public static async Task<T> ReadAsAsync<T>(this HttpContent content)
    {
        return await JsonSerializer.DeserializeAsync<T>(await content.ReadAsStreamAsync(), GetTradeSerializerOptions())
               ?? throw new ArgumentNullException(nameof(content));
    }

    private static JsonSerializerOptions GetTradeSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        options.Converters.Add(new JsonStringEnumConverter());

        return options;
    }
}