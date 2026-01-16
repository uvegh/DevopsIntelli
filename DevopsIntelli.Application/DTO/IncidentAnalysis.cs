using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.DTO;

public record IncidentAnalysis
{
    public Guid IncidentId { get; init; }
    public string Summary { get; init; } = string.Empty;
    public List<string> RootCauses { get; init; } = new();
    public List<string> Recommendations { get; init; } = new();
    public List<Guid> SimilarIncidentIds { get; init; } = new();
    public double ConfidenceScore { get; init; }
    public DateTime AnalyzedAt { get; init; }
}
