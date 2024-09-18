// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// The trade party for this supply chain consignment.
/// </summary>
public class TradeParty
{
    /// <summary>
    /// A unique identifier of this trade party.
    /// </summary>
    public IList<IDType> Id { get; set; }

    /// <summary>
    /// The name, expressed as text, for this trade party.
    /// </summary>
    public IList<TextType> Name { get; set; }

    /// <summary>
    /// A code specifying the role of this trade party.
    /// </summary>
    public IList<CodeType> RoleCode { get; set; }

    /// <summary>
    /// The postal address for this trade party.
    /// </summary>
    public TradeAddress PostalAddress { get; set; }

    /// <summary>
    /// The postal address for this trade party.
    /// </summary>
    public UniversalCommunication Telephone { get; set; }

    /// <summary>
    /// The postal address for this trade party.
    /// </summary>
    public UniversalCommunication EmailAddress { get; set; }

    /// <summary>
    /// The details of a authoritative signatory person of a trade party.
    /// </summary>
    public AuthoritativeSignatoryPerson AuthoritativeSignatoryPerson { get; set; }

    /// <summary>
    /// Trade Contact DefinedContactDetails
    /// </summary>
    public IList<TradeContact> DefinedContactDetails { get; set; }
}