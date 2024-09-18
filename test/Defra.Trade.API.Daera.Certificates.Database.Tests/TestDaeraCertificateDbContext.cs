// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Database.Context;

namespace Defra.Trade.API.Daera.Certificates.Database.Tests;

public class TestDaeraCertificateDbContext : DaeraCertificateDbContext
{
    public ModelBuilder? ModelBuilder { get; private set; }
    public DbContextOptionsBuilder? OptionsBuilder { get; private set; }

    public void TestSaveChanges()
    {
        base.SaveChanges();
    }

    public async Task<int> TestSaveChangesAsync()
    {
        int result = await base.SaveChangesAsync();
        return result;
    }

    public void TestOnModelCreating(ModelBuilder modelBuilder)
    {
        ModelBuilder = modelBuilder;
        base.OnModelCreating(modelBuilder);
    }

    public void TestOnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        OptionsBuilder = optionsBuilder;
        base.OnConfiguring(optionsBuilder);
    }
}
