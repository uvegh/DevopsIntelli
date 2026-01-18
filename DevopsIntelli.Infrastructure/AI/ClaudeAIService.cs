using DevopsIntelli.Application.common.Interface;
using DevopsIntelli.Application.DTO;
using DevopsIntelli.Domain.Common.Entities;
using DevopsIntelli.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace DevopsIntelli.Infrastructure.AI;

 public class ClaudeOptions{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "claude-3-5-sonnet-20241022";
    public int MaxToken = 4096;
        }
public class ClaudeAIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly DevopsIntelliDBContext _context;
    private readonly ILogger _logger;
    private readonly ClaudeOptions _claudeOptions;




    public ClaudeAIService(HttpClient httpClient, DevopsIntelliDBContext context, IOptions<ClaudeOptions>claudeOptions, ILogger<ClaudeAIService> logger)
    {
        _httpClient=httpClient;
        _claudeOptions = claudeOptions.Value;
        _context = context;
        _logger = logger;

        _httpClient.BaseAddress = new Uri("https://api.anthropic.com/v1/");
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _claudeOptions.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");


    }

    public async Task<IncidentAnalysis> AnalyzeIncidentAsync(Guid incidentId, CancellationToken ct = default)
    {
        var incident = await _context.Incident.FirstOrDefaultAsync(i=> i.Id==incidentId,ct);
        if (incident == null) throw new NotFoundException(nameof(incident), incidentId);
    }

    public Task<LogAnalysisResult> AnalyzeLogsAsync(string logContent, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

   
}
