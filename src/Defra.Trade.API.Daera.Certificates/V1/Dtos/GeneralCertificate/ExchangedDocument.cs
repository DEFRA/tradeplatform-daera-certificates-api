// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// The header document information for a use of this master message assembly.
/// </summary>
public class ExchangedDocument
{
    /// <summary>
    /// The unique identifier of this exchanged document.
    /// </summary>
    [Required]
    public IDType Id { get; set; }

    /// <summary>
    /// The code specifying the type of exchanged document.
    /// </summary>
    [Required]
    public CodeType TypeCode { get; set; }

    /// <summary>
    /// The date, time, date time or other date time value for the issuance of this exchanged document.
    /// </summary>
    [Required]
    public DateTimeType IssueDateTime { get; set; }

    /// <summary>
    /// The unique identifier of this exchanged document from the Trader.
    /// </summary>
    public IDType TraderAssignedId { get; set; }

    /// <summary>
    /// Other documents referenced by this exchanged document.
    /// </summary>
    public IList<ReferencedDocument> ReferenceDocument { get; set; }

    /// <summary>
    /// A signatory document authentication for this exchanged document.
    /// </summary>
    [Required]
    public DocumentAuthentication PrimarySignatoryAuthentication { get; set; }

    /// <summary>
    /// The party that issues this exchanged document.
    /// </summary>
    [Required]
    public TradeParty Issuer { get; set; }
}