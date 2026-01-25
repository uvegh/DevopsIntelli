using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.DTO;

public  record LogAnalysisResult
{
    public string Summary { get; init; } = string.Empty;
    public List<string> KeyFindings { get; init; } = new();
    public string SeverityLevel { get; init; } = string.Empty;
    public List<string> AffectedComponents { get; init; } = new();
}
