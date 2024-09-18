// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

public class LogisticsTransportEquipment
{
    public IDType Id { get; set; }

    public IList<LogisticsSeal> AffixedSeal { get; set; }

    public IList<TransportSettingTemperature> TemperatureSetting { get; set; }
}