// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Database.Context;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Database.Services.Interfaces;

namespace Defra.Trade.API.Daera.Certificates.Database.Services;

/// <inheritdoc />
public class DbHealthCheckService(DaeraCertificateDbContext context) : IDbHealthCheckService
{
    private readonly DaeraCertificateDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    /// <inheritdoc />
    public async Task<DatabaseInfo> GetContextInfo()
    {
        return await _context.GetInfo();
    }
}