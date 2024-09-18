// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.API.Daera.Certificates.Infrastructure;

[ExcludeFromCodeCoverage]
public sealed class InternalApimSettings
{
    public static string SectionName { get; set; } = "Apim:Internal";
    public string Authority { get; set; }
    public string BaseUrl { get; set; }
    public string DaeraInternalCertificateStoreApi { get; set; } = "/certificates-store/v1";
    public string SubscriptionKey { get; set; }
    public string SubscriptionKeyHeaderName { get; set; }
}