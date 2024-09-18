// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.Database.Models;

public class BaseTable
{
    [Required]
    public DateTimeOffset CreatedOn { get; set; }

    [Required]
    [MaxLength(100)]
    public string CreatedBy { get; set; }

    [MaxLength(10)]
    public string CreatedSystem { get; set; }

    public DateTimeOffset LastUpdatedOn { get; set; }

    [MaxLength(100)]
    public string LastUpdatedBy { get; set; }

    [MaxLength(10)]
    public string LastUpdatedSystem { get; set; }

    [Required]
    public bool IsActive { get; set; }
}