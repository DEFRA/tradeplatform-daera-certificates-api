// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Queries;

namespace Defra.Trade.API.Daera.Certificates.V1.Validation;

public class GetGeneralCertificateSummariesQueryValidator : AbstractValidator<GetGeneralCertificateSummariesQuery>
{
    public GetGeneralCertificateSummariesQueryValidator() : base()
    {
        RuleFor(x => x.SortOrder)
            .IsInEnum();

        RuleFor(x => x.PageNumber)
            .InclusiveBetween(1, int.MaxValue);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100000);

        RuleFor(x => x.ModifiedSince)
            .GreaterThanOrEqualTo(new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .LessThanOrEqualTo(new DateTimeOffset(2999, 12, 31, 0, 0, 0, TimeSpan.Zero))
            .When(x => x.ModifiedSince != null);
    }
}