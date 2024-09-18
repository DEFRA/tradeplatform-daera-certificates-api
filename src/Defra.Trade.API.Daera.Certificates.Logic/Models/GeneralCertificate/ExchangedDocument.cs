// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

public class ExchangedDocument
{
    public IDType Id { get; set; }

    public CodeType TypeCode { get; set; }

    public DateTimeType IssueDateTime { get; set; }

    public IDType TraderAssignedId { get; set; }

    public IList<ReferencedDocument> ReferenceDocument { get; set; }

    public DocumentAuthentication PrimarySignatoryAuthentication { get; set; }

    public TradeParty Issuer { get; set; }
}