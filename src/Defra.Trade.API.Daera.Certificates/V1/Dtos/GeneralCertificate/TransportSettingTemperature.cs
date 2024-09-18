// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// Temperature settings for a transport movement, such as a required storage temperature range.
/// </summary>
public class TransportSettingTemperature
{
    /// <summary>
    /// The measure of the value of this transport setting temperature, such as a temperature value of ten degrees Celsius.
    /// </summary>
    [Required]
    public TemperatureUnitMeasure Value { get; set; }

    /// <summary>
    /// The code specifying the type of transport setting temperature
    /// </summary>
    [Required]
    public CodeType TypeCode { get; set; }
}