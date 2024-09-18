// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

public class LogisticsLocation
{
    public IList<IDType> Id { get; set; }

    public TextType Name { get; set; }

    public TradeAddress LocationAddress { get; set; }
}