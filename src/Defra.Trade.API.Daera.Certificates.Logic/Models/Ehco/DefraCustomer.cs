// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

/// <summary>
/// The Defra customer's details
/// </summary>
public class DefraCustomer
{
    /// <summary>
    /// The Defra customer's organisation id
    /// </summary>
    public Guid OrgId { get; set; }

    /// <summary>
    /// The Defra customer's user id
    /// </summary>
    public Guid UserId { get; set; }
}