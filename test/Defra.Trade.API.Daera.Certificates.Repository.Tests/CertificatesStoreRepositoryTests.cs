// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Defra.Trade.API.Daera.Certificates.Database.Context;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Database.Models.Enum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Defra.Trade.API.Daera.Certificates.Repository.Tests;

public class CertificatesStoreRepositoryTests
{
    [Fact]
    public async Task GetGeneralCertificateSummary_ValidId_Ok()
    {
        // arrange
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var data = fixture.Create<GeneralCertificate>();

        var token = new CancellationTokenSource().Token;

        await using var context = CreateContext();

        context.GeneralCertificate.Add(data);
        await context.SaveChangesAsync(token);

        var sut = new CertificatesStoreRepository(context);

        // act
        var actual = await sut.GetGeneralCertificateSummaryAsync(data.GeneralCertificateId, token);

        // assert
        actual.Should().BeEquivalentTo(data);
    }

    [Fact]
    public async Task GetGeneralCertificateWithEnrichmentAsync_ValidId_Ok()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var data = fixture.Create<Database.Models.GeneralCertificate>();

        var token = new CancellationTokenSource().Token;

        await using var context = CreateContext();

        context.GeneralCertificate.Add(data);
        await context.SaveChangesAsync(token);

        var sut = new CertificatesStoreRepository(context);

        // act
        var actual = await sut.GetGeneralCertificateWithEnrichmentAsync(data.GeneralCertificateId, token);

        // assert
        actual.Should().BeEquivalentTo(data);
    }

    [Fact]
    public async Task GetGeneralCertificateWithEnrichmentAsync_ValidId_But_No_EnrichmentData_Returns_Null()
    {
        // Arrange
        var fixture = new Fixture();
        var data = fixture.Build<Database.Models.GeneralCertificate>()
            .Without(x => x.GeneralCertificateDocument)
            .Without(x => x.EnrichmentData)
            .Create();

        var token = new CancellationTokenSource().Token;

        await using var context = CreateContext();

        context.GeneralCertificate.Add(data);
        await context.SaveChangesAsync(token);

        var sut = new CertificatesStoreRepository(context);

        // act
        var actual = await sut.GetGeneralCertificateWithEnrichmentAsync(data.GeneralCertificateId, token);

        // assert
        actual.Should().BeNull();
    }

    [Theory]
    [InlineData(DateSource.GeneralCertificate, DateSource.GeneralCertificate)]
    [InlineData(DateSource.GeneralCertificate, DateSource.EnrichmentData)]
    [InlineData(DateSource.EnrichmentData, DateSource.GeneralCertificate)]
    [InlineData(DateSource.EnrichmentData, DateSource.EnrichmentData)]
    public async Task GetGeneralCertificateSummariesAsync_WithQuery_PagedSummaries(DateSource createdAtSource, DateSource lastUpdatedSource)
    {
        var createdAt = RandomDate();
        var notCreatedAt = RandomDate(min: createdAt);
        var lastUpdated = RandomDate();
        var notLastUpdated = RandomDate(max: lastUpdated);
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt
            .Without(ed => ed.GeneralCertificate)
            .With(ed => ed.CreatedOn, createdAtSource == DateSource.EnrichmentData ? createdAt : notCreatedAt)
            .With(ed => ed.LastUpdatedOn, lastUpdatedSource == DateSource.EnrichmentData ? lastUpdated : notLastUpdated)
        );
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        fixture.Customize<GeneralCertificate>(opt => opt
            .With(ed => ed.CreatedOn, createdAtSource == DateSource.GeneralCertificate ? createdAt : notCreatedAt)
            .With(ed => ed.LastUpdatedOn, lastUpdatedSource == DateSource.GeneralCertificate ? lastUpdated : notLastUpdated)
        );

        var data = fixture
            .CreateMany<GeneralCertificate>(3)
            .ToList();

        var query = new GeneralCertificateSummariesQuery
        {
            PageNumber = 1,
            PageSize = 2,
            SortOrder = SortOrder.Asc,
            ModifiedSince = DateTimeOffset.MinValue
        };

        var token = new CancellationTokenSource().Token;

        await using var context = CreateContext();

        context.GeneralCertificate.AddRange(data);
        await context.SaveChangesAsync(token);

        var sut = new CertificatesStoreRepository(context);

        // act
        var actual = await sut.GetGeneralCertificateSummariesAsync(query, token);

        // assert
        actual.Data.Should().HaveCount(2);
        actual.TotalRecords.Should().Be(3);
        foreach (var summary in actual.Data)
        {
            summary.CreatedOn.Should().Be(createdAt);
            summary.LastUpdated.Should().Be(lastUpdated);
        }
    }

    [Fact]
    public async Task GetGeneralCertificateDocument_ValidId_Ok()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));


        var data = fixture.Build<Database.Models.GeneralCertificate>().Create();
        string documentId = data.GeneralCertificateDocument.FirstOrDefault().DocumentId;

        var token = new CancellationTokenSource().Token;

        await using var context = CreateContext();

        context.GeneralCertificate.Add(data);
        await context.SaveChangesAsync(token);

        var sut = new CertificatesStoreRepository(context);

        // act
        var actual = await sut.GetGeneralCertificateWithDocumentsAsync(data.GeneralCertificateId, documentId, token);

        // assert
        actual.Should().BeEquivalentTo(data);
    }

    [Fact]
    public async Task GetGeneralCertificateDocuments_ValidId_Ok()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));


        var data = fixture.Build<Database.Models.GeneralCertificate>().Create();

        var token = new CancellationTokenSource().Token;

        await using var context = CreateContext();

        context.GeneralCertificate.Add(data);
        await context.SaveChangesAsync(token);

        var sut = new CertificatesStoreRepository(context);

        // act
        var actual = await sut.GetGeneralCertificateWithDocumentsAsync(data.GeneralCertificateId, token);

        // assert
        actual.Should().BeEquivalentTo(data);
    }

    private static DaeraCertificateDbContext CreateContext()
    {
        var builder = new DbContextOptionsBuilder<DaeraCertificateDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        return new DaeraCertificateDbContext(builder.Options);
    }

    private static DateTimeOffset RandomDate(DateTimeOffset? min = null, DateTimeOffset? max = null)
    {
        return new(Random.Shared.NextInt64(min?.Ticks ?? DateTime.MinValue.Ticks, max?.Ticks ?? DateTime.MaxValue.Ticks), TimeSpan.Zero);
    }

    public enum DateSource
    {
        GeneralCertificate,
        EnrichmentData
    }
}