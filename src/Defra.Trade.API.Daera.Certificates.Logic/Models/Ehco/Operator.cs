// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Text.Json.Serialization;

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

/// <summary>
/// The operator for this supply chain consignment.
/// </summary>
public class Operator
{
    /// <summary>
    /// The name, expressed as text, for this operator.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A code specifying the postcode of this operator address.
    /// </summary>
    public string Postcode { get; set; }

    /// <summary>
    /// The first free form line, expressed as text, of this operator address.
    /// </summary>
    public string LineOne { get; set; }

    /// <summary>
    /// The second free form line, expressed as text, of this operator address.
    /// </summary>
    public string LineTwo { get; set; }

    /// <summary>
    /// The city name, expressed as text, of the city, town or village of this operator address.
    /// </summary>
    public string CityName { get; set; }

    /// <summary>
    /// The unique code of a country for this operator address.
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    /// The telephone number for this operator.
    /// </summary>
    public string Telephone { get; set; }

    /// <summary>
    /// Email communication for this operator.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Operator traces.
    /// </summary>
    [JsonPropertyName("TRACES")]
    public OperatorTraces Traces { get; set; }
}