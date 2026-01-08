using DevopsIntelli.Domain.Common.Enum;
using DevopsIntelli.Domain.Common.Interface;
using System;


namespace DevopsIntelli.Domain.Events
{
    public  sealed record IncidentDetectedEvent : IDomainEvent
    {
        public DateTime OccuredAt { get; init; }

        public DateTime? UpdatedAt { get; init; }
        public Severity Severity { get; init; }
        public string AffectedService { get; init; } = string.Empty;

        public Guid EventId { get; init; }
        public string TenantId { get; init; }
    public Guid IncidentId { get; init; }
        //public TimeSpan ResolutionTime { get; init; } 
    }
}
