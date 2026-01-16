using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.DTO;

public record AnalysisResult
{
    public   Guid IncidentId { get; init; }
    public DateTime DetectedAt { get; init; }
    public string Analysis { get; init; } = string.Empty;
    public string[] Recommendations { get; init; } = Array.Empty<string>();
    public DateTime AnalyzedAt { get; init; }

}
