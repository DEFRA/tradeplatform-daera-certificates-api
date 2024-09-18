// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

/// <summary>
/// A supply chain consignment specified for a use of this master message assembly.
/// </summary>
public class SupplyChainConsignment
{
    /// <summary>
    /// A unique identifier for this supply chain consignment.
    /// </summary>
    public IDType Id { get; set; }

    /// <summary>
    /// The customs identifiers for this supply chain consignment.
    /// </summary>
    public IList<IDType> CustomsID { get; set; }

    /// <summary>
    /// The date, time, date time, or other date time value when this consignment will exit,
    /// or has exited from the last port, airport, or border post of the country of export.
    /// </summary>
    [Required]
    public DateTimeType ExportExitDateTime { get; set; }

    /// <summary>
    /// The consignor party for this supply chain consignment.
    /// </summary>
    [Required]
    public TradeParty Consignor { get; set; }

    /// <summary>
    /// The consignee party for this supply chain consignment.
    /// </summary>
    [Required]
    public TradeParty Consignee { get; set; }

    /// <summary>
    /// The operator responsible for this supply chain consignment.
    /// </summary>
    [Required]
    public TradeParty OperatorResponsibleForConsignment { get; set; }

    /// <summary>
    /// The border control post at which this consignment will arrive on crossing the border.
    /// </summary>
    [Required]
    public LogisticsLocation BorderControlPostLocation { get; set; }

    /// <summary>
    /// The baseport location at which this supply chain consignment is to be loaded on a means of transport
    /// according to the transport contract.
    /// </summary>
    [Required]
    public LogisticsLocation DispatchLocation { get; set; }

    /// <summary>
    /// The baseport location at which this supply chain consignment is to be unloaded on a means of transport
    /// according to the transport contract.
    /// </summary>
    [Required]
    public LogisticsLocation DestinationLocation { get; set; }

    /// <summary>
    /// Logistics transport equipment utilized for this supply chain consignment.
    /// </summary>
    public IList<LogisticsTransportEquipment> UsedTransportEquipment { get; set; }

    /// <summary>
    /// A logistics transport movement specified for this supply chain consignment.
    /// </summary>
    public IList<LogisticsTransportMovement> MainCarriageTransportMovement { get; set; }

    /// <summary>
    /// The origin country for this supply chain consignment.
    /// </summary>
    public IList<TradeCountry> OriginCountry { get; set; }

    /// <summary>
    /// The import country for this supply chain consignment.
    /// </summary>
    [Required]
    public TradeCountry ImportCountry { get; set; }

    /// <summary>
    /// The export country for this supply chain consignment.
    /// </summary>
    [Required]
    public TradeCountry ExportCountry { get; set; }
}