// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Database.Services;

namespace Defra.Trade.API.Daera.Certificates.Database.Tests.Services;

public class DbHealthCheckServiceTests
{
    [Fact]
    public void Ctor_NullArgs_Throws()
    {
        // Act && Assert
        Assert.Throws<ArgumentNullException>(
            () => new DbHealthCheckService(null));
    }
}
