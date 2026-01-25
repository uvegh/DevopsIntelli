

namespace DevopsIntelli.Application.DTO;


// DTOs for Claude API
public record ClaudeResponse
{
    public ClaudeContent[] Content { get; init; } = Array.Empty<ClaudeContent>();
}

public record ClaudeContent
{
    public string Type { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
}

public record AnalysisJson
{
    public string Summary { get; init; } = string.Empty;
    public List<string> RootCauses { get; init; } = new();
    public List<string> Recommendations { get; init; } = new();
    public double ConfidenceScore { get; init; }
}
