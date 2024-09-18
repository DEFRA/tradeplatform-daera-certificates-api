// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;

/// <summary>
/// Organisation details for enriched General Certificate payload
/// </summary>
public class Organisation
{
    /// <summary>
    /// The id for the Organisation.
    /// </summary>
    public Guid DefraCustomerId { get; set; }

    /// <summary>
    /// The ReMoS id associated with the organisation
    /// </summary>
    public string RmsId { get; set; }

    /// <summary>
    /// The name of the organisation
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The email address for the organisation
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The telephone number for the organisation
    /// </summary>
    public string Telephone { get; set; }

    /// <summary>
    /// The address of the organisation
    /// </summary>
    public Address Address { get; set; }
}