// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Logic.Services;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests;

public class DateTimeProviderTests
{
    [Fact]
    public void Now_WhenCalled_ReturnsUtcNow()
    {
        // Arrange
        var sut = new DateTimeProvider();

        // Act
        var utcNow = DateTimeOffset.UtcNow;
        var result = sut.Now;

        // Assert
        result.Should().BeBefore(utcNow.AddSeconds(2));
        result.Should().BeAfter(utcNow.AddSeconds(-2));
    }
}