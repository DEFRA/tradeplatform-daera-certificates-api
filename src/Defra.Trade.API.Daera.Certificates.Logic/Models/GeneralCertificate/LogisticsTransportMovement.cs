// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

public class LogisticsTransportMovement
{
    public CodeType ModeCode { get; set; }

    public IDType Id { get; set; }

    public LogisticsTransportMeans UsedTransportMeans { get; set; }
}