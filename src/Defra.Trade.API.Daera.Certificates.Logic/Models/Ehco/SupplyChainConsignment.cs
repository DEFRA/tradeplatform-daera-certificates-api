// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

/// <summary>
/// A supply chain consignment specified for a use of this master message assembly.
/// </summary>
public class SupplyChainConsignment
{
    /// <summary>
    /// The date, time, date time, or other date time value when this consignment will exit,
    /// or has exited from the last port, airport, or border post of the country of export.
    /// </summary>
    public string ExportExitDateTime { get; set; }

    /// <summary>
    /// The consignor party for this supply chain consignment.
    /// </summary>
    public Consignor Consignor { get; set; }

    /// <summary>
    /// The consignee party for this supply chain consignment.
    /// </summary>
    public Consignee Consignee { get; set; }

    /// <summary>
    /// The dispatch location for this supply chain consignment.
    /// </summary>
    public Location DispatchLocation { get; set; }

    /// <summary>
    /// The destination location for this supply chain consignment.
    /// </summary>
    public Location DestinationLocation { get; set; }

    /// <summary>
    /// The operator for this supply chain consignment.
    /// </summary>
    public Operator OperatorResponsibleForConsignment { get; set; }

    /// <summary>
    /// The Goods Movement Reference (GMR) number
    /// </summary>
    public string CustomsId { get; set; }

    /// <summary>
    /// Logistics transport means utilized for this supply chain consignment.
    /// </summary>
    public LogisticsTransportMeans UsedTransportMeans { get; set; }

    /// <summary>
    /// The border control post location code
    /// </summary>
    public string BorderControlPostLocation { get; set; }

    /// <summary>
    /// Logistics transport equipment utilized for this supply chain consignment.
    /// </summary>
    public LogisticsTransportEquipment UsedTransportEquipment { get; set; }

    /// <summary>
    /// The country of import for this supply chain consignment.
    /// </summary>
    public string ImportCountry { get; set; }

    /// <summary>
    /// The country of export for this supply chain consignment.
    /// </summary>
    public string ExportCountry { get; set; }
}