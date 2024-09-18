// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Infrastructure;

public class ApimExternalApisSettings
{
    public const string OptionsName = "Apim:External";

    public string BaseUrl { get; set; }

    public string DaeraCertificatesApiPathV1 { get; set; } = "/daera-certificates/v1";

    public string DaeraCertificatesApiUrlV1 => BaseUrl + DaeraCertificatesApiPathV1;
}