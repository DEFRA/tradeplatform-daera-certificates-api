// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Database.Context;

public partial class DaeraCertificateDbContext
{
    public override int SaveChanges()
    {
        throw new InvalidOperationException("method not allowed, use SaveChangesAsync.");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(true, cancellationToken);
    }
}
