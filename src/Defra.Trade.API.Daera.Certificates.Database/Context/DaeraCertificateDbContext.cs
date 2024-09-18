// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Defra.Trade.API.Daera.Certificates.Database.Context;

public partial class DaeraCertificateDbContext : DbContext
{
    public virtual DbSet<GeneralCertificate> GeneralCertificate { get; set; }

    public DaeraCertificateDbContext()
    {
    }

    public DaeraCertificateDbContext(DbContextOptions<DaeraCertificateDbContext> options)
        : base(options)
    {
        // Constructor to set models.
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("gcs");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DaeraCertificateDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    public async Task<DatabaseInfo> GetInfo()
    {
        bool canConnect = await base.Database.CanConnectAsync();
        string databaseName = base.Database.GetDbConnection().Database;
        var appliedMigrations = await base.Database.GetAppliedMigrationsAsync();
        var pendingMigrations = await base.Database.GetPendingMigrationsAsync();

        var databaseInfo = new DatabaseInfo()
        {
            CanConnect = canConnect,
            DatabaseName = databaseName,
            CurrentMigration = appliedMigrations.Any() ? appliedMigrations.Last() : null,
            PendingMigrations = pendingMigrations
        };

        return databaseInfo;
    }
}