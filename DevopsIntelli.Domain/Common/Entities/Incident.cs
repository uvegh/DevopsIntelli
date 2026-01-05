using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Domain.Common.Entities;

public class Incident:BaseEntity
{
    private Incident()
    {

    }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Severity Severity { get; private set; }
    public IncidentStatus IncidentStatus { get; private set; }

}
