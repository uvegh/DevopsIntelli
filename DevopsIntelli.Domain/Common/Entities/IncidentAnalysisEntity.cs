using System.Text.Json;

namespace DevopsIntelli.Domain.Common.Entities;

/// <summary>
/// Cached AI analysis results for an incident
/// WHY: Avoid re-analyzing the same incident type repeatedly
/// INTERVIEW: "I cache AI analysis to reduce API costs and improve response times"
/// </summary>
public class IncidentAnalysis : BaseEntity
{
    public Guid IncidentId { get; private set; }
    public string Summary { get; private set; } = string.Empty;
    public string RootCausesJson { get; private set; } = string.Empty; // JSON array
    public string RecommendationsJson { get; private set; } = string.Empty; // JSON array
    public string SimilarIncidentIdsJson { get; private set; } = string.Empty; // JSON array
    public double ConfidenceScore { get; private set; }
    public DateTime AnalyzedAt { get; private set; }

    // Navigation
    public Incident Incident { get; private set; } = null!;

    private IncidentAnalysis() { } // EF Core

    public static IncidentAnalysis Create(
        Guid incidentId,
        string summary,
        List<string> rootCauses,
        List<string> recommendations,
        List<Guid> similarIncidentIds,
        double confidenceScore)
    {
        return new IncidentAnalysis
        {
            Id = Guid.NewGuid(),
            IncidentId = incidentId,
            Summary = summary,
            RootCausesJson = JsonSerializer.Serialize(rootCauses),
            RecommendationsJson = JsonSerializer.Serialize(recommendations),
            SimilarIncidentIdsJson = JsonSerializer.Serialize(similarIncidentIds),
            ConfidenceScore = confidenceScore,
            AnalyzedAt = DateTime.UtcNow
        };
    }

    //public List<string> GetRootCauses()
    //    => JsonSerializer.Deserialize<List<string>>(RootCausesJson) ?? new();


    //public List<string> GetRecommendations()
    //    => JsonSerializer.Deserialize<List<string>>(RecommendationsJson) ?? new();

    //public List<Guid> GetSimilarIncidentIds()
    //    => JsonSerializer.Deserialize<List<Guid>>(SimilarIncidentIdsJson) ?? new();
}