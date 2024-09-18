// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Database.Models;

public class EnrichmentData : BaseTable
{
    public Guid Id { get; set; }

    public virtual GeneralCertificate GeneralCertificate { get; set; }

    public int SchemaVersion { get; set; }

    public string Data { get; set; }
}