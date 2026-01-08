

using DevopsIntelli.Domain.Common.Interface;

namespace DevopsIntelli.Domain.Events;

public record IncidentResolvedEvent:IDomainEvent
{
    public DateTime OccuredAt { get; init; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; init; }
    

    public Guid EventId { get; init; }=
    Guid.NewGuid();

    
}
