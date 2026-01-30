using DevopsIntelli.Application.DTO;
using DevopsIntelli.Domain.Common.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DevopsIntelli.Application.Features.Incidents.Commands
{
    public record CreateIncidentCommand(
        [Required]
           string ,
        [Required]
     Severity Severity,
    [ Required]
    DetectionMethod DetectionMethod,
   string? Source) : IRequest<CreateIncidentResult>;
}
