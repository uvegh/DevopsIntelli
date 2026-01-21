namespace DevopsIntelli.Infrastructure.Persistence.Configurations;

using DevopsIntelli.Domain.Common.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class IncidentAnalysisConfiguration : IEntityTypeConfiguration<IncidentAnalysisEntity>
{
    public void Configure(EntityTypeBuilder<IncidentAnalysisEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Summary)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.RootCausesJson)
            .HasColumnType("jsonb") // PostgreSQL JSON type
            .IsRequired();

        builder.Property(x => x.RecommendationsJson)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.SimilarIncidentIdsJson)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.ConfidenceScore)
            .HasPrecision(3, 2); // 0.00 to 1.00

        builder.Property(x => x.AnalyzedAt)
            .IsRequired();

        // Relationship
        builder.HasOne(x => x.Incident)
            .WithMany()
            .HasForeignKey(x => x.IncidentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for fast lookups
        builder.HasIndex(x => x.IncidentId)
            .IsUnique(); // One analysis per incident
    }

   
}