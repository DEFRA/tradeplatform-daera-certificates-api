// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using Defra.Trade.API.Daera.Certificates.Database.Context;

namespace Defra.Trade.API.Daera.Certificates.Database.Tests;

public class DaeraCertificateDbContextTests
{
    private readonly IFixture _fixture;
    private readonly TestDaeraCertificateDbContext _context;

    public DaeraCertificateDbContextTests()
    {
        _context = new TestDaeraCertificateDbContext();
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
    }

    [Fact]
    public void Ctors_EnsureNotNullAndCorrectExceptionParameterName()
    {
        var assertion = new GuardClauseAssertion(_fixture);
        assertion.Verify(typeof(DaeraCertificateDbContext).GetConstructors());
    }

    [Fact]
    public void ShouldSaveChangesAsync_As_expected()
    {
        //Act
        Func<Task> result = async () => await _context.TestSaveChangesAsync();

        //Assert
        _ = result.Should().NotThrowAsync();
    }

    [Fact]
    public void WhenCalled_SaveChanges_ShouldThrowException()
    {
        //Act
        Action result = () => _context.TestSaveChanges();

        //Assert
        result.Should().Throw<InvalidOperationException>("method not allowed, use SaveChangesAsync.");
    }

    [Fact]
    public void Should_Initialize_Context()
    {
        // Act
        Action result = () => _context.TestOnModelCreating(new ModelBuilder());

        // Assert
        result.Should().NotThrow();
    }

    [Fact]
    public void Should_Configure_Context()
    {
        // Act
        Action result = () => _context.TestOnConfiguring(new DbContextOptionsBuilder());

        // Assert
        result.Should().NotThrow();
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldRun_AsExpected()
    {
        // Assert
        var dbContext = new Mock<DaeraCertificateDbContext>();
        dbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        await dbContext.Object.SaveChangesAsync(CancellationToken.None);

        // Assert
        dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
