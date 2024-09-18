// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;

/// <summary>
/// Establishment details for enriched General Certificate payload
/// </summary>
public class Establishment
{
    /// <summary>
    /// The id for the establishment
    /// </summary>
    public Guid EstablishmentId { get; set; }

    /// <summary>
    /// The customer id associated with the estbalishment
    /// </summary>
    public Guid DefraCustomerId { get; set; }

    /// <summary>
    /// The ReMoS id associated with the establishment
    /// </summary>
    public string RmsId { get; set; }

    /// <summary>
    /// The name of the establishment
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The address of the establishment
    /// </summary>
    public Address Address { get; set; }
}