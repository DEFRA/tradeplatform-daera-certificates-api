// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;
using Defra.Trade.Common.Api.Dtos;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// General Certificate payload for Defra to share with DAERA to facilitate the movement of goods between Great Britain
/// and Northern Ireland under the Retail Movement Scheme as part of the Windsor Framework.
/// </summary>
public class GeneralCertificate : ResourceBase
{
    /// <summary>
    /// The header document information for a use of this master message assembly.
    /// </summary>
    [Required]
    public ExchangedDocument ExchangedDocument { get; set; }

    /// <summary>
    /// A supply chain consignment specified for a use of this master message assembly.
    /// </summary>
    [Required]
    public SupplyChainConsignment SupplyChainConsignment { get; set; }
}