// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

public class TradeParty
{
    public IList<IDType> Id { get; set; }

    public IList<TextType> Name { get; set; }

    public IList<CodeType> RoleCode { get; set; }

    public TradeAddress PostalAddress { get; set; }

    public UniversalCommunication Telephone { get; set; }

    /// <summary>
    /// The postal address for this trade party.
    /// </summary>
    public UniversalCommunication EmailAddress { get; set; }

    public AuthoritativeSignatoryPerson AuthoritativeSignatoryPerson { get; set; }

    public IList<TradeContact> DefinedContactDetails { get; set; }
}