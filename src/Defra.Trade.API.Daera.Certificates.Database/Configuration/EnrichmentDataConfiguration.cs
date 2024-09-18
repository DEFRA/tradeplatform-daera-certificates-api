// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Defra.Trade.API.Daera.Certificates.Database.Configuration;

public class EnrichmentDataConfiguration : IEntityTypeConfiguration<EnrichmentData>
{
    public void Configure(EntityTypeBuilder<EnrichmentData> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.GeneralCertificate)
            .WithOne(e => e.EnrichmentData)
            .HasForeignKey<EnrichmentData>("GeneralCertificateId")
            .IsRequired();
    }
}