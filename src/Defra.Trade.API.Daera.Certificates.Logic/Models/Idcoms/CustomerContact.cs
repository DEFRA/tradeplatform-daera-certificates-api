// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;

/// <summary>
/// Customer details for enriched General Certificate payload
/// </summary>
public class CustomerContact
{
    /// <summary>
    /// The user id for the trader
    /// </summary>
    public Guid DefraCustomerId { get; set; }

    /// <summary>
    /// The name of the trader
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The email address for the trader
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The telephone number for the user
    /// </summary>
    public string Telephone { get; set; }

    /// <summary>
    /// The address of the trader
    /// </summary>
    public Address Address { get; set; }
}