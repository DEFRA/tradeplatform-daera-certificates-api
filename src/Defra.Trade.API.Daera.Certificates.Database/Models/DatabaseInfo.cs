// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Generic;

namespace Defra.Trade.API.Daera.Certificates.Database.Models;

public sealed class DatabaseInfo
{
    public bool CanConnect { get; set; }
    public string DatabaseName { get; set; }
    public string CurrentMigration { get; set; }
    public IEnumerable<string> PendingMigrations { get; set; }

    public override string ToString()
    {
        return $"Can Connect: {CanConnect}, " +
            $"Database Name: {DatabaseName}, " +
            $"Current Migration: {CurrentMigration}, " +
            $"Pending Migrations: {PendingMigrations}";
    }
}