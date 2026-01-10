using DevopsIntelli.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Infrastructure.Persistence.Configurations;

public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
{
    public void Configure(EntityTypeBuilder<Incident> builder)
    {
        builder.ToTable("Incidents");
        builder.HasKey(i => i.Id);

        // Properties
        builder.Property(i => i.Id)
            .ValueGeneratedNever(); // We generate Guids in code, not DB

        builder.Property(i => i.TenantId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(5000);

        // Enum stored as string (readable in database)
        builder.Property(i => i.Severity)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(i => i.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(i => i.DetectionMethod)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(i => i.AffectedService)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.DetectedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Analysis)
            .HasMaxLength(10000); // Nullable

        builder.Property(i => i.AnalysisConfidence)
            .HasPrecision(3, 2); // e.g., 0.95

        builder.Property(i => i.RemdiationSteps).HasConversion(
            v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
            v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new List<string>()).HasColumnType("jsonb");
        builder.Property(i => i.CreatedAt).IsRequired();
        builder.Property(i => i.UpdatedAt);
        builder.Property(i => i.DetectedAt);
        builder.Property(i => i.ResolvedAt);
        builder.HasIndex(i => i.TenantId);
        
    }
}
