// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Defra.Trade.API.Daera.Certificates.Database.Models;

public class GeneralCertificate : BaseTable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string GeneralCertificateId { get; set; }

    [Required]
    public int SchemaVersion { get; set; }

    [Required]
    public string Data { get; set; }

    public virtual EnrichmentData EnrichmentData { get; set; }

    public virtual IList<GeneralCertificateDocument> GeneralCertificateDocument { get; set; }
}