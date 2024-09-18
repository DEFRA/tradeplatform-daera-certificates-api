// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

/// <summary>
/// The Ehco General Certificate payload
/// </summary>
public class EhcoGeneralCertificateApplication
{
    /// <summary>
    /// The header document information for a use of this master message assembly.
    /// </summary>
    public ExchangedDocument ExchangedDocument { get; set; }
    /// <summary>
    /// A supply chain consignment specified for a use of this master message assembly.
    /// </summary>
    public SupplyChainConsignment SupplyChainConsignment { get; set; }
}
