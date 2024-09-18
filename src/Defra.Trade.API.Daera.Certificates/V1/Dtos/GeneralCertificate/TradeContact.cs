// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;

public class TradeContact
{
    public IDType Id { get; set; }

    [Required]
    public TextType PersonName { get; set; }
}