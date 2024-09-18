// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A signatory document authentication for this exchanged document.
/// </summary>
public class DocumentAuthentication
{
    /// <summary>
    /// The actual date, time, date time, or other date time value of this document authentication.
    /// </summary>
    [Required]
    public DateTimeType ActualDateTime { get; set; }

    /// <summary>
    /// A note included in this exchanged document.
    /// </summary>
    public IList<DocumentClause> IncludedDocumentClause { get; set; }

    /// <summary>
    /// The party that sends this exchanged document.
    /// </summary>
    [Required]
    public TradeParty ProvidingTradeParty { get; set; }
}