using DevopsIntelli.Application.DTO;
using DevopsIntelli.Domain.Common.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.Features.Incidents.Commands;

public class CreateIncidentCommandHandler:IRequestHandler<CreateIncidentCommand,CreateIncidentResult>
{

    

    public Task<CreateIncidentResult> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
