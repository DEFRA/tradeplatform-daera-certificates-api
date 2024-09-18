// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;

/// <summary>Retrieve DateTime information.</summary>
public interface IDateTimeProvider
{
    /// <summary>Get the DateTime as it is now.</summary>
    DateTimeOffset Now { get; }
}