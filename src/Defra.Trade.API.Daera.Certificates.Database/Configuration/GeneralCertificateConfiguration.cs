// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Defra.Trade.API.Daera.Certificates.Database.Configuration;

public class GeneralCertificateConfiguration : IEntityTypeConfiguration<GeneralCertificate>
{
    public void Configure(EntityTypeBuilder<GeneralCertificate> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.EnrichmentData)
            .WithOne(e => e.GeneralCertificate)
            .HasForeignKey<EnrichmentData>("GeneralCertificateId")
            .IsRequired(false);

        builder.HasIndex(e => e.GeneralCertificateId)
             .IsClustered(false);

        builder.HasIndex(e => e.LastUpdatedOn)
             .IsClustered(false);
    }
}