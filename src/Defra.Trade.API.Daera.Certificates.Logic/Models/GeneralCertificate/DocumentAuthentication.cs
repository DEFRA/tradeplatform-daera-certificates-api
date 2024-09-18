// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

public class DocumentAuthentication
{
    public DateTimeType ActualDateTime { get; set; }

    public IList<DocumentClause> IncludedDocumentClause { get; set; }

    public TradeParty ProvidingTradeParty { get; set; }
}