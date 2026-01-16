using DevopsIntelli.Application.DTO;


namespace DevopsIntelli.Application.common.Interface;

public interface  IAIService
{
    /// <summary>
    /// Analyzes the specified incident asynchronously and returns a detailed analysis result.  
    /// </summary>
    /// <param name="incidentId">The unique identifier of the incident to analyze.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the analysis operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IncidentAnalysis"/>
    /// object with details about the analyzed incident.</returns>
    Task<IncidentAnalysis> AnalyzeIncidentAsync(Guid incidentId, CancellationToken ct=default);
    Task<LogAnalysisResult> AnalyzeLogsAsync(string logContent, CancellationToken ct = default);
}
