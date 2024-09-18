// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Database.Models;

namespace Defra.Trade.API.Daera.Certificates.Database.Services.Interfaces;

/// <summary>
/// Database health check service
/// </summary>
public interface IDbHealthCheckService
{
    /// <summary>
    /// Gets information on the database context for health monitoring
    /// </summary>
    /// <remarks>
    /// Cannot be unit tested due to EF InMemory databases not using the underlying calls
    ///     that are present in the deployed scenario where relational databases are used
    /// </remarks>
    /// <returns>The database context information</returns>
    Task<DatabaseInfo> GetContextInfo();
}