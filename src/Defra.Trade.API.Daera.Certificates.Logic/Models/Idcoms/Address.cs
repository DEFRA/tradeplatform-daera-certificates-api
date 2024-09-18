// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;

/// <summary>
/// Address details for enriched General Certificate payload
/// </summary>
public class Address
{
    /// <summary>
    /// Address line 1 of the Address
    /// </summary>
    public string AddressLine1 { get; set; }

    /// <summary>
    /// Address line 2 of the Address
    /// </summary>
    public string AddressLine2 { get; set; }

    /// <summary>
    /// Address line 3  of the Address
    /// </summary>
    public string AddressLine3 { get; set; }

    /// <summary>
    /// Address line 4 of the Address
    /// </summary>
    public string AddressLine4 { get; set; }

    /// <summary>
    /// Address line 5  of the Address
    /// </summary>
    public string AddressLine5 { get; set; }

    /// <summary>
    /// The town  of the Address
    /// </summary>
    public string Town { get; set; }

    /// <summary>
    /// The country the address is located in
    /// </summary>
    public Country Country { get; set; }

    /// <summary>
    /// Postal code.
    /// </summary>
    public string PostCode { get; set; }

    /// <summary>
    /// County.
    /// </summary>
    public string County { get; set; }
}