using DevopsIntelli.Application.DTO;
using DevopsIntelli.Domain.Common.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.Features.Incidents.Commands;

public class CreateIncidentCommandHandler:IRequestHandler<CreateIncidentCommand,CreateIncidentResult>
{

    public async Task Handle( CreateIncidentCommand req)
    {
        var incident = Incident.

    }
}
