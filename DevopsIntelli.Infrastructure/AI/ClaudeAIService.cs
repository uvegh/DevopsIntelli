namespace DevOpsIntelligence.Infrastructure.AI;

using DevopsIntelli.Application.common.Exceptions;
using DevopsIntelli.Domain.Common.Entities;
using DevopsIntelli.Infrastructure.Data;
using System.Text.Json;



/// <summary>
/// Configuration for Claude API
/// </summary>
public class ClaudeOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "claude-3-5-sonnet-20241022";
    public int MaxTokens { get; set; } = 4096;
}

/// <summary>
/// Claude API implementation of AI service
/// WHY: Claude excels at complex reasoning and log analysis
/// INTERVIEW: "I integrate with Claude API for advanced reasoning capabilities"
/// </summary>
public class ClaudeAIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly DevopsIntelliDBContext _context;
    private readonly ILogger<ClaudeAIService> _logger;
    private readonly ClaudeOptions _options;

    public ClaudeAIService(
        HttpClient httpClient,
         DevopsIntelliDBContext context,
        ILogger<ClaudeAIService> logger,
        IOptions<ClaudeOptions> options)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
        _options = options.Value;

        // Configure HTTP client
        _httpClient.BaseAddress = new Uri("https://api.anthropic.com/v1/");
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _options.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    }

    public async Task<IncidentAnalysis> AnalyzeIncidentAsync(
        Guid incidentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting AI analysis for incident {IncidentId}", incidentId);

        try
        {
            // 1. Fetch incident from database
            var incident = await _context.Incident
                .FirstOrDefaultAsync(i => i.Id == incidentId, cancellationToken);

            if (incident == null)
            {
                throw new NotFoundException(nameof(Incident), incidentId);
            }

            // 2. Build context for Claude
            var prompt = BuildIncidentAnalysisPrompt(incident);

            // 3. Call Claude API
            var request = new
            {
                model = _options.Model,
                max_tokens = _options.MaxTokens,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync("messages", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ClaudeResponse>(cancellationToken);

            // 4. Parse Claude's response
            var analysis = ParseAnalysisResponse(result!.Content[0].Text, incidentId);

            _logger.LogInformation(
                "Completed AI analysis for incident {IncidentId} with confidence {Confidence}",
                incidentId,
                analysis.ConfidenceScore);

            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to analyze incident {IncidentId}", incidentId);
            throw;
        }
    }

    public async Task<LogAnalysisResult> AnalyzeLogsAsync(
        string logContent,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Analyzing log content ({Length} chars)", logContent.Length);

        try
        {
            var prompt = BuildLogAnalysisPrompt(logContent);

            var request = new
            {
                model = _options.Model,
                max_tokens = 2048,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync("messages", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ClaudeResponse>(cancellationToken);

            return ParseLogAnalysisResponse(result!.Content[0].Text);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to analyze log content");
            throw;
        }
    }

    private string BuildIncidentAnalysisPrompt(Incident incident)
    {
        return $@"You are a DevOps expert analyzing a production incident. Provide a structured analysis.

Incident Details:
- ID: {incident.Id}
- Message: {incident.Description}
- Severity: {incident.Severity}
- Detected: {incident.DetectedAt}
- Detection Method: {incident.DetectionMethod}

Provide your analysis in this JSON format:
{{
  ""summary"": ""Brief 2-3 sentence summary"",
  ""rootCauses"": [""cause1"", ""cause2""],
  ""recommendations"": [""recommendation1"", ""recommendation2""],
  ""confidenceScore"": 0.85
}}

Focus on actionable insights.";
    }

    private string BuildLogAnalysisPrompt(string logContent)
    {
        return $@"Analyze these application logs and identify key issues:

{logContent}

Provide analysis in this JSON format:
{{
  ""summary"": ""Brief summary of what's happening"",
  ""keyFindings"": [""finding1"", ""finding2""],
  ""severityLevel"": ""High/Medium/Low"",
  ""affectedComponents"": [""component1"", ""component2""]
}}";
    }

    private IncidentAnalysis ParseAnalysisResponse(string responseText, Guid incidentId)
    {
        try
        {
            // Extract JSON from Claude's response (it might have markdown)
            var jsonStart = responseText.IndexOf('{');
            var jsonEnd = responseText.LastIndexOf('}') + 1;
            var json = responseText.Substring(jsonStart, jsonEnd - jsonStart);

            var jsonSerialerOpt = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var parsed = JsonSerializer.Deserialize<AnalysisJson>(json, jsonSerialerOpt);

            return new IncidentAnalysis
            {
                IncidentId = incidentId,
                Summary = parsed!.Summary,
                RootCauses = parsed.RootCauses,
                Recommendations = parsed.Recommendations,
                ConfidenceScore = parsed.ConfidenceScore,
                SimilarIncidentIds = new List<Guid>(), // TODO: Implement similarity search
                AnalyzedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse AI response: {Response}", responseText);

            // Fallback analysis
            return new IncidentAnalysis
            {
                IncidentId = incidentId,
                Summary = "AI analysis failed to parse",
                RootCauses = new List<string> { "Analysis incomplete" },
                Recommendations = new List<string> { "Manual investigation required" },
                ConfidenceScore = 0.0,
                AnalyzedAt = DateTime.UtcNow
            };
        }
    }

    private LogAnalysisResult ParseLogAnalysisResponse(string responseText)
    {
        try
        {
            var start = responseText.IndexOf("{");
            var end = responseText.IndexOf("}")+1;

            var json = responseText.Substring(start, end - start);
            var jsonSerializerOpt = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<LogAnalysisResult>(json, jsonSerializerOpt)!;


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse log analysis response");
            return new LogAnalysisResult
            {
                Summary = "Analysis failed",
                KeyFindings = new List<string> { "Unable to parse response" },
                SeverityLevel = "Unknown",
                AffectedComponents = new List<string>()
            };
        }
    }
}

