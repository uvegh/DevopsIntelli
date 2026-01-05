using DevopsIntelli.Domain.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Domain.Common.Entities;

public class BaseEntity
{
    public string TenantId { get; protected set; } = string.Empty;
    private readonly List<IDomainEvent> _domainEvents = new();
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void RaiseDomainEvent(IDomainEvent domainEvent) {
        _domainEvents.Add(domainEvent);

    }

    public void ClearDomainEvent()
    {
        _domainEvents.Clear();
    }


}
