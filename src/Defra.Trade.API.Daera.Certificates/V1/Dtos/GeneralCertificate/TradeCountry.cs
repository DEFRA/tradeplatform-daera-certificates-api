// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// The trade country for this trade product.
/// </summary>
public class TradeCountry
{
    /// <summary>
    /// A unique identifier for this trade country.
    /// </summary>
    [Required]
    public IDType Code { get; set; }

}