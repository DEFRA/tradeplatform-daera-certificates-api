// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;

/// <summary>
/// Country details for enriched General Certificate payload
/// </summary>
public class Country
{
    /// <summary>
    /// The name of the country
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The code for the country
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// The country sub division name
    /// </summary>
    public string SubDivisionName { get; set; }

    /// <summary>
    /// The country sub division code
    /// </summary>
    public string SubDivisionCode { get; set; }
}