// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// The trade address for this trade party.
/// </summary>
public class TradeAddress
{
    /// <summary>
    /// A code specifying the postcode of this trade address.
    /// </summary>
    [Required]
    public CodeType Postcode { get; set; }

    /// <summary>
    /// The first free form line, expressed as text, of this trade address.
    /// </summary>
    [Required]
    public TextType LineOne { get; set; }

    /// <summary>
    /// The second free form line, expressed as text, of this trade address.
    /// </summary>
    public TextType LineTwo { get; set; }

    /// <summary>
    /// The third free form line, expressed as text, of this trade address.
    /// </summary>
    public TextType LineThree { get; set; }

    /// <summary>
    /// The fourth free form line, expressed as text, of this trade address.
    /// </summary>
    public TextType LineFour { get; set; }

    /// <summary>
    /// The fifth free form line, expressed as text, of this trade address.
    /// </summary>
    public TextType LineFive { get; set; }

    /// <summary>
    /// The name, expressed as text, of the city, town or village of this trade address.
    /// </summary>
    [Required]
    public TextType CityName { get; set; }

    /// <summary>
    /// The unique identifier of a country for this trade address.
    /// </summary>
    [Required]
    public IDType CountryCode { get; set; }

    /// <summary>
    /// A name, expressed as text, of the country for this trade address.
    /// </summary>
    public TextType CountryName { get; set; }

    /// <summary>
    /// A unique identifier of the country sub-division for this trade address.
    /// </summary>
    public IDType CountrySubDivisionCode { get; set; }

    /// <summary>
    /// A name, expressed as text, of the sub-division of a country for this trade address.
    /// </summary>
    public TextType CountrySubDivisionName { get; set; }

    /// <summary>
    /// A code specifying the type of this trade address, such as business address or home address.
    /// </summary>
    [Required]
    public CodeType TypeCode { get; set; }
}