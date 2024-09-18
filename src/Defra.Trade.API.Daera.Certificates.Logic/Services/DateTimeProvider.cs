// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services;

/// <inheritdoc cref="IDateTimeProvider"/>
public class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc/>
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}