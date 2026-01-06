using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.DTO;

public record  CreateIncidentResult
{
    public Guid IncidentId { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool WasAlreadyProcessed { get; init; }
}
