// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.Common.Api.Dtos;
using Defra.Trade.Common.ExternalApi.Auditing.Data;
using Defra.Trade.Common.ExternalApi.Auditing.Models;
using Defra.Trade.Common.ExternalApi.Auditing.Models.Enums;

namespace Defra.Trade.API.Daera.Certificates.IntegrationTests.Helpers;

public static class TestVerificationHelpers
{
    public static void VerifyAuditLogged(
        this Mock<IAuditRepository> auditRepoMock,
        AuditLogType logType,
        Guid? clientId,
        string systemRequestId,
        string httpMethod,
        string path,
        string queryString,
        int statusCode,
        DateTimeOffset loggedOnOrAfter,
        bool hasRequestBody,
        bool hasResponseBody,
        string clientIPAddress)
    {
        Func<AuditLog, bool> isAuditMatch = (a) =>
        {
            if (a == null)
                return false;

            return a.LogType == logType
                   && a.OrganisationId == Guid.Empty
                   && a.ApplicantId == Guid.Empty
                   && a.ClientId == clientId
                   && a.RequestId == null
                   && a.SystemRequestId == systemRequestId
                   && !string.IsNullOrWhiteSpace(a.TraceId)
                   && a.HttpMethod == httpMethod
                   && (a.Path?.Equals(path, StringComparison.OrdinalIgnoreCase) ?? path == null)
                   && (a.QueryString?.Equals(queryString, StringComparison.OrdinalIgnoreCase) ?? queryString == null)
                   && a.HttpStatusCode == statusCode
                   && a.Timestamp >= loggedOnOrAfter
                   && string.IsNullOrEmpty(a.Data.RequestData) != hasRequestBody
                   && string.IsNullOrEmpty(a.Data.ResponseData) != hasResponseBody
                   && (a.ClientIPAddress?.Equals(clientIPAddress, StringComparison.OrdinalIgnoreCase) ?? clientIPAddress == null);
        };

        auditRepoMock.Verify(r =>
            r.CreateAsync(
                It.Is<AuditLog>(a => isAuditMatch(a))));
    }

    public static void VerifyForbidden(this CommonProblemDetails problem)
    {
        problem.VerifyResponse(HttpStatusCode.Forbidden);
    }

    public static void VerifyNotFound(this CommonProblemDetails problem)
    {
        problem.VerifyResponse(HttpStatusCode.NotFound);
    }

    public static void VerifyBadRequest(this CommonProblemDetails problem)
    {
        problem.VerifyResponse(HttpStatusCode.BadRequest);
    }

    public static void VerifyResponse(this CommonProblemDetails problem, HttpStatusCode statusCode)
    {
        problem.Status.Should().Be((int)statusCode);
        problem.Title.Should().NotBeNullOrWhiteSpace();
        problem.Type.Should().Match(t => Uri.IsWellFormedUriString(t, UriKind.RelativeOrAbsolute));
    }
}