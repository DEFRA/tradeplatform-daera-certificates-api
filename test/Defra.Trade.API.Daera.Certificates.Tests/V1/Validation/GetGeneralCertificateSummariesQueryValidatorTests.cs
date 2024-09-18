// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Dtos.Enums;
using Defra.Trade.API.Daera.Certificates.V1.Queries;
using Defra.Trade.API.Daera.Certificates.V1.Validation;
using FluentValidation.TestHelper;

namespace Defra.Trade.API.Daera.Certificates.Tests.V1.Validation;

public class GetGeneralCertificateSummariesQueryValidatorTests
{
    [Fact]
    public void Validate_Valid_OK()
    {
        var itemUnderTest = new GetGeneralCertificateSummariesQueryValidator();

        var fixture = new Fixture();

        var request = fixture.Build<GetGeneralCertificateSummariesQuery>()
            .With(a => a.PageNumber, 1)
            .With(a => a.PageSize, 1)
            .With(a => a.ModifiedSince, new DateTimeOffset(2023, 02, 01, 0, 0, 0, TimeSpan.Zero))
            .With(a => a.SortOrder, SortOrder.Asc)
            .Create();

        var result = itemUnderTest.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_Invalid_PageNumber_PageSize_Values_Error()
    {
        var itemUnderTest = new GetGeneralCertificateSummariesQueryValidator();

        var fixture = new Fixture();

        var request = fixture.Build<GetGeneralCertificateSummariesQuery>()
            .With(a => a.PageNumber, -1)
            .With(a => a.PageSize, 0)
            .Create();

        var result = itemUnderTest.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
            .WithErrorMessage("'Page Number' must be between 1 and 2147483647. You entered -1.");
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorMessage("'Page Size' must be between 1 and 100000. You entered 0.");
    }

    [Fact]
    public void Validate_Invalid_ModifiedSince_MinDate_Value_Error()
    {
        var itemUnderTest = new GetGeneralCertificateSummariesQueryValidator();

        var fixture = new Fixture();

        var request = fixture.Build<GetGeneralCertificateSummariesQuery>()
            .With(a => a.ModifiedSince, new DateTimeOffset(2022, 02, 01, 0, 0, 0, TimeSpan.Zero))
            .Create();

        var result = itemUnderTest.TestValidate(request);

        var minDate = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);

        result.ShouldHaveValidationErrorFor(x => x.ModifiedSince)
            .WithErrorMessage($"'Modified Since' must be greater than or equal to '{minDate}'.");
    }

    [Fact]
    public void Validate_Invalid_ModifiedSince_MaxDate_Value_Error()
    {
        var itemUnderTest = new GetGeneralCertificateSummariesQueryValidator();

        var fixture = new Fixture();

        var request = fixture.Build<GetGeneralCertificateSummariesQuery>()
            .With(a => a.ModifiedSince, new DateTimeOffset(3000, 01, 01, 0, 0, 0, TimeSpan.Zero))
            .Create();

        var result = itemUnderTest.TestValidate(request);

        var maxDate = new DateTimeOffset(2999, 12, 31, 0, 0, 0, TimeSpan.Zero);

        result.ShouldHaveValidationErrorFor(x => x.ModifiedSince)
            .WithErrorMessage($"'Modified Since' must be less than or equal to '{maxDate}'.");
    }
}