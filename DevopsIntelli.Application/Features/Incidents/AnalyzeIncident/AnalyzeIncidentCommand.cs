using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.Features.Incidents.AnalyzeIncident;

public record class AnalyzeIncidentCommand:IRequest<AnalysisResult>
{
}
