// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Database.Models.Enum;
using FluentAssertions;
using Xunit;

namespace Defra.Trade.API.Daera.Certificates.Repository.Tests;

public class RepositoryQueryExtensionsTests
{
    [Theory]
    [InlineData(DateSource.GeneralCertificate, DateSource.GeneralCertificate)]
    [InlineData(DateSource.GeneralCertificate, DateSource.EnrichmentData)]
    [InlineData(DateSource.EnrichmentData, DateSource.GeneralCertificate)]
    [InlineData(DateSource.EnrichmentData, DateSource.EnrichmentData)]
    public void SelectSummaries_Returns_Summary(DateSource createdAtSource, DateSource lastUpdatedSource)
    {
        var fixture = new Fixture();
        var createdAt = RandomDate();
        var notCreatedAt = RandomDate(min: createdAt);
        var lastUpdated = RandomDate();
        var notLastUpdated = RandomDate(max: lastUpdated);
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

        var items = fixture
            .CreateMany<GeneralCertificate>(5)
            .AsQueryable();

        var result = items.SelectSummaries().ToList();

        result.Should().HaveCount(5);
        var actual = result[0];
        actual.GeneralCertificateId.Should().Be(items.First().GeneralCertificateId);
        actual.CreatedOn.Should().Be(createdAt);
        actual.LastUpdated.Should().Be(lastUpdated);
    }

    [Fact]
    public void WhereMatched_ModifiedSinceNotSpecified_AllRecords()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var items = fixture
            .CreateMany<GeneralCertificate>(30)
            .AsQueryable();

        var query = new GeneralCertificateSummariesQuery();

        var result = items.WhereMatched(query).ToList();

        result.Should().HaveCount(30);
    }

    [Fact]
    public void WhereMatched_ModifiedSince_LessThan_GeneralCertificate_LastUpdatedOn_And_EnrichmentData_CorrectResults()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var expected = fixture
            .Build<GeneralCertificate>()
            .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 2, 11, 10, 30, 30, 0, TimeSpan.Zero))
            .With(gc => gc.EnrichmentData,
                fixture
                    .Build<EnrichmentData>()
                    .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 2, 13, 11, 30, 30, 0, TimeSpan.Zero))
                    .Without(ed => ed.GeneralCertificate)
                    .Create())
            .Create();

        var items = new List<GeneralCertificate>
        {
            fixture
                .Build<GeneralCertificate>()
                .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 5, TimeSpan.Zero))
                .With(gc => gc.EnrichmentData,
                    fixture
                        .Build<EnrichmentData>()
                        .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 5, TimeSpan.Zero))
                        .Without(ed => ed.GeneralCertificate)
                        .Create())
                .Create(),

            fixture
                .Build<GeneralCertificate>()
                .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 6, TimeSpan.Zero))
                .With(gc => gc.EnrichmentData,
                    fixture
                        .Build<EnrichmentData>()
                        .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 6, TimeSpan.Zero))
                        .Without(ed => ed.GeneralCertificate).Create())
                .Create(),

            expected
        }.AsQueryable();

        var query = new GeneralCertificateSummariesQuery
        {
            ModifiedSince = new DateTimeOffset(2023, 2, 10, 0, 0, 0, 0, TimeSpan.Zero)
        };

        var result = items.WhereMatched(query).ToList();

        result.Should().HaveCount(1);
        var actual = result[0];
        actual.GeneralCertificateId.Should().Be(expected.GeneralCertificateId);
        actual.LastUpdatedOn.Should().Be(expected.LastUpdatedOn);
    }

    [Fact]
    public void WhereMatched_ModifiedSince_GreaterThan_GeneralCertificate_LastUpdatedOn_But_LessThan_EnrichmentData_LastUpdatedOn_Dates_CorrectResults()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var expected = fixture
            .Build<GeneralCertificate>()
            .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 2, 9, 10, 30, 30, 0, TimeSpan.Zero))
            .With(gc => gc.EnrichmentData,
                fixture
                    .Build<EnrichmentData>()
                    .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 2, 13, 11, 30, 30, 0, TimeSpan.Zero))
                    .Without(ed => ed.GeneralCertificate)
                    .Create())
            .Create();

        var items = new List<GeneralCertificate>
        {
            fixture
                .Build<GeneralCertificate>()
                .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 5, TimeSpan.Zero))
                .With(gc => gc.EnrichmentData,
                    fixture
                        .Build<EnrichmentData>()
                        .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 5, TimeSpan.Zero))
                        .Without(ed => ed.GeneralCertificate)
                        .Create())
                .Create(),

            fixture
                .Build<GeneralCertificate>()
                .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 6, TimeSpan.Zero))
                .With(gc => gc.EnrichmentData,
                    fixture
                        .Build<EnrichmentData>()
                        .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 6, TimeSpan.Zero))
                        .Without(ed => ed.GeneralCertificate).Create())
                .Create(),

            expected
        }.AsQueryable();

        var query = new GeneralCertificateSummariesQuery
        {
            ModifiedSince = new DateTimeOffset(2023, 2, 10, 0, 0, 0, 0, TimeSpan.Zero)
        };

        var result = items.WhereMatched(query).ToList();

        result.Should().HaveCount(1);
        var actual = result[0];
        actual.GeneralCertificateId.Should().Be(expected.GeneralCertificateId);
        actual.LastUpdatedOn.Should().Be(expected.LastUpdatedOn);
    }

    [Fact]
    public void WhereMatched_ModifiedSince_LessThan_GeneralCertificate_LastUpdatedOn_But_GreaterThan_EnrichmentData_LastUpdatedOn_Dates_CorrectResults()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var expected = fixture
            .Build<GeneralCertificate>()
            .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 2, 11, 10, 30, 30, 0, TimeSpan.Zero))
            .With(gc => gc.EnrichmentData,
                fixture
                    .Build<EnrichmentData>()
                    .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 2, 9, 11, 30, 30, 0, TimeSpan.Zero))
                    .Without(ed => ed.GeneralCertificate)
                    .Create())
            .Create();

        var items = new List<GeneralCertificate>
        {
            fixture
                .Build<GeneralCertificate>()
                .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 5, TimeSpan.Zero))
                .With(gc => gc.EnrichmentData,
                    fixture
                        .Build<EnrichmentData>()
                        .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 5, TimeSpan.Zero))
                        .Without(ed => ed.GeneralCertificate)
                        .Create())
                .Create(),

            fixture
                .Build<GeneralCertificate>()
                .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 6, TimeSpan.Zero))
                .With(gc => gc.EnrichmentData,
                    fixture
                        .Build<EnrichmentData>()
                        .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 6, TimeSpan.Zero))
                        .Without(ed => ed.GeneralCertificate).Create())
                .Create(),

            expected
        }.AsQueryable();

        var query = new GeneralCertificateSummariesQuery
        {
            ModifiedSince = new DateTimeOffset(2023, 2, 10, 0, 0, 0, 0, TimeSpan.Zero)
        };

        var result = items.WhereMatched(query).ToList();

        result.Should().HaveCount(1);
        var actual = result[0];
        actual.GeneralCertificateId.Should().Be(expected.GeneralCertificateId);
        actual.LastUpdatedOn.Should().Be(expected.LastUpdatedOn);
    }

    [Fact]
    public void WhereMatched_ModifiedSince_GreaterThan_GeneralCertificate_LastUpdatedOn_But_GreaterThan_EnrichmentData_LastUpdatedOn_Dates_CorrectResults()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var expected = fixture
            .Build<GeneralCertificate>()
            .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 2, 8, 10, 30, 30, 0, TimeSpan.Zero))
            .With(gc => gc.EnrichmentData,
                fixture
                    .Build<EnrichmentData>()
                    .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 2, 9, 11, 30, 30, 0, TimeSpan.Zero))
                    .Without(ed => ed.GeneralCertificate)
                    .Create())
            .Create();

        var items = new List<GeneralCertificate>
        {
            fixture
                .Build<GeneralCertificate>()
                .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 5, TimeSpan.Zero))
                .With(gc => gc.EnrichmentData,
                    fixture
                        .Build<EnrichmentData>()
                        .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 5, TimeSpan.Zero))
                        .Without(ed => ed.GeneralCertificate)
                        .Create())
                .Create(),

            fixture
                .Build<GeneralCertificate>()
                .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 6, TimeSpan.Zero))
                .With(gc => gc.EnrichmentData,
                    fixture
                        .Build<EnrichmentData>()
                        .With(ed => ed.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 6, TimeSpan.Zero))
                        .Without(ed => ed.GeneralCertificate).Create())
                .Create(),

            expected
        }.AsQueryable();

        var query = new GeneralCertificateSummariesQuery
        {
            ModifiedSince = new DateTimeOffset(2023, 2, 10, 0, 0, 0, 0, TimeSpan.Zero)
        };

        var result = items.WhereMatched(query).ToList();

        result.Should().HaveCount(0);
    }

    [Fact]
    public void Ordered_DefaultOrder_Asc()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var expectedFirst = fixture
            .Build<GeneralCertificate>()
            .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 6, TimeSpan.Zero))
            .Create();

        var expectedSecond = fixture
            .Build<GeneralCertificate>()
            .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 7, TimeSpan.Zero))
            .Create();

        var items = new List<GeneralCertificate>
        {
            expectedSecond,
            expectedFirst
        }.AsQueryable();

        var query = new GeneralCertificateSummariesQuery();

        var result = items.Ordered(query).ToList();

        result.Should().HaveCount(2);
        result[0].Should().BeSameAs(expectedFirst);
        result[1].Should().BeSameAs(expectedSecond);
    }

    [Fact]
    public void Ordered_Desc_Correct()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var expectedFirst = fixture
            .Build<GeneralCertificate>()
            .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 10, TimeSpan.Zero))
            .Without(x => x.GeneralCertificateDocument)
            .Create();

        var expectedSecond = fixture
            .Build<GeneralCertificate>()
            .With(gc => gc.LastUpdatedOn, new DateTimeOffset(2023, 1, 2, 3, 4, 5, 9, TimeSpan.Zero))
            .Without(x => x.GeneralCertificateDocument)
            .Create();

        var items = new List<GeneralCertificate>
        {
            expectedSecond,
            expectedFirst
        }.AsQueryable();

        var query = new GeneralCertificateSummariesQuery
        {
            SortOrder = SortOrder.Desc
        };

        var result = items.Ordered(query).ToList();

        result.Should().HaveCount(2);
        result[0].Should().BeSameAs(expectedFirst);
        result[1].Should().BeSameAs(expectedSecond);
    }

    [Fact]
    public void Paginated_WithPageNumberAndCount_Success()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var items = fixture
            .CreateMany<GeneralCertificate>(30)
            .AsQueryable();

        var query = new GeneralCertificateSummariesQuery
        {
            PageNumber = 2,
            PageSize = 6
        };

        var result = items.Paginated(query).ToList();

        result.Should().HaveCount(6);
        result[0].Should().BeSameAs(items.Skip(6).Take(1).Single());
    }

    [Fact]
    public void WhereHasEnrichmentData_MultipleCertificates_OnlyThoseWithEnrichment()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var items = new List<GeneralCertificate>
        {
            fixture.Build<GeneralCertificate>().Without(c => c.EnrichmentData).Create(),
            fixture.Create<GeneralCertificate>()
        }.AsQueryable();

        var result = items.WhereHasEnrichmentData().ToList();

        result.Should().HaveCount(1);
        result[0].Should().BeSameAs(items.Last());
    }

    [Fact]
    public void FindGeneralCertificate_MultipleCertificates_MatchingGcId()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var items = new List<GeneralCertificate>
        {
            fixture.Build<GeneralCertificate>().With(c => c.GeneralCertificateId, "abc").Create(),
            fixture.Build<GeneralCertificate>().With(c => c.GeneralCertificateId, "123").Create(),
        }.AsQueryable();

        var result = items.FindGeneralCertificate("abc").ToList();

        result.Should().HaveCount(1);
        result[0].GeneralCertificateId.Should().Be("abc");
    }

    [Fact]
    public void IncludeEnrichmentData_SingleCertificate_IncludesEnrichment()
    {
        var fixture = new Fixture();
        fixture.Customize<EnrichmentData>(opt => opt.Without(ed => ed.GeneralCertificate));
        fixture.Customize<GeneralCertificateDocument>(opt => opt.Without(ed => ed.GeneralCertificate));

        var items = fixture.CreateMany<GeneralCertificate>(1).AsQueryable();

        var result = items.IncludeEnrichmentData().ToList();

        result.Should().HaveCount(1);
        result[0].EnrichmentData.Should().NotBeNull();
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