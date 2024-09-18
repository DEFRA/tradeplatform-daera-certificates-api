// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models;

public class QueueRuntimeInfo
{
    public DateTimeOffset AccessedAt { get; set; }

    public string ErrorMessage { get; set; }

    public int PendingMigrationsCount { get; set; }

    public IReadOnlyCollection<string> PendingMigrations { get; set; }
}