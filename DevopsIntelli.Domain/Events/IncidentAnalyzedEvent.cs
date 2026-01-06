

using DevopsIntelli.Domain.Common.Interface;

namespace DevopsIntelli.Domain.Events;

public record IncidentAnalyzedEvent:IDomainEvent
{
    public DateTime OccuredAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; init; }

    public Guid EventId { get; init; }=
    Guid.NewGuid();
    public string TenantId { get; init; } = string.Empty;
    public double Confidence { get; init; }
   

}
