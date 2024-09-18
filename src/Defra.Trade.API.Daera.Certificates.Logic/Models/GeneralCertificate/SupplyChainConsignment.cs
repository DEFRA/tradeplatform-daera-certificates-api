// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

public class SupplyChainConsignment
{
    public IDType Id { get; set; }

    public IList<IDType> CustomsId { get; set; }

    public DateTimeType ExportExitDateTime { get; set; }

    public TradeParty Consignor { get; set; }

    public TradeParty Consignee { get; set; }

    public TradeParty OperatorResponsibleForConsignment { get; set; }

    public LogisticsLocation BorderControlPostLocation { get; set; }

    public LogisticsLocation DispatchLocation { get; set; }

    public LogisticsLocation DestinationLocation { get; set; }

    public IList<LogisticsTransportEquipment> UsedTransportEquipment { get; set; }

    public IList<LogisticsTransportMovement> MainCarriageTransportMovement { get; set; }

    public IList<TradeCountry> OriginCountry { get; set; }

    public TradeCountry ImportCountry { get; set; }

    public TradeCountry ExportCountry { get; set; }
}