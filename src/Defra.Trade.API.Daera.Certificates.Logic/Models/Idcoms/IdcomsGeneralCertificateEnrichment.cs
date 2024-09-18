// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;

/// <summary>
/// The General Certificate Enrichment payload
/// </summary>
public class IdcomsGeneralCertificateEnrichment
{
    /// <summary>
    /// The General Certificate Id
    /// </summary>
    public string GcId { get; set; }

    /// <summary>
    /// The trader details for the General Certificate
    /// </summary>
    public CustomerContact Applicant { get; set; }

    /// <summary>
    /// The organisation details for the General Certificate
    /// </summary>
    public IReadOnlyCollection<Organisation> Organisations { get; set; }

    /// <summary>
    /// The establishment list for a General Certificate
    /// </summary>
    public IReadOnlyCollection<Establishment> Establishments { get; set; }
}