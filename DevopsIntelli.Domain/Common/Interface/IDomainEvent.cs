using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Domain.Common.Interface;

public interface IDomainEvent
{
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public Guid EventId {get;}

}
