// src/DevOpsIntelli.Application/Features/Incidents/Commands/CreateIncident/CreateIncidentCommandHandler.cs
namespace DevOpsIntelligence.Application.Features.Incidents.Commands.CreateIncident;

using DevopsIntelli.Application.common.Interface;
using DevopsIntelli.Domain.Common.Entities;
using DevopsIntelli.Domain.Common.Enums;

using MediatR;
using Microsoft.Extensions.Logging;

public record CreateIncidentCommand : IRequest<Guid>
{
    public required string Message { get; init; }
    public required Severity Severity { get; init; }
    public required DetectionMethod DetectionMethod { get; init; }
    public string? Source { get; init; }
}

public class CreateIncidentCommandHandler : IRequestHandler<CreateIncidentCommand, Guid>
{
    private readonly IIncidentRepository _incidentRepo;
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorService _vectorStore;
    private readonly ILogger<CreateIncidentCommandHandler> _logger;

    public CreateIncidentCommandHandler(
        IIncidentRepository incidentRepo,
        IEmbeddingService embeddingService,
        IVectorService vectorStore,
        ILogger<CreateIncidentCommandHandler> logger)
    {
        _incidentRepo = incidentRepo;
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _logger = logger;
    }

    public async Task<Guid> Handle(
        CreateIncidentCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating incident: {Message}", request.Message);

        // STEP 1: Create incident entity
        var incident = Incident.Create(
            request.
            request.Message,
            request.Severity,
            request.DetectionMethod);

        // STEP 2: Save to PostgreSQL
      await   _incidentRepo.AddAsync(incident, cancellationToken);
      

        _logger.LogInformation("Incident saved to PostgreSQL with Id: {IncidentId}", incident.Id);

        try
        {
            // STEP 3: Generate embedding for the incident message
            _logger.LogDebug("Generating embedding for incident message");
            var embedding = await _embeddingService.GenerateEmbeddingAsync(
                incident.Message,
                cancellationToken);

            // STEP 4: Store vector in Qdrant with metadata
            _logger.LogDebug("Storing vector in Qdrant");
            await _vectorStore.StoreVectorAsync(
                id: incident.Id, // ← SAME ID as PostgreSQL!
                embedding: embedding,
                metadata: new Dictionary<string, object>
                {
                    { "incident_id", incident.Id.ToString() }, // For linking
                    { "severity", incident.Severity.ToString() },
                    { "detection_method", incident.DetectionMethod.ToString() },
                    { "detected_at", incident.DetectedAt.ToString("O") },
                    { "message_preview", incident.Message.Substring(0, Math.Min(100, incident.Message.Length)) }
                },
                cancellationToken);

            _logger.LogInformation(
                "Vector stored in Qdrant for incident {IncidentId}",
                incident.Id);
        }
        catch (Exception ex)
        {
            // Vector storage failed, but incident is already in PostgreSQL
            // Log error but don't fail the entire operation
            _logger.LogError(
                ex,
                "Failed to store vector in Qdrant for incident {IncidentId}. Incident still saved in PostgreSQL.",
                incident.Id);

            // You could add a flag to the incident: NeedsVectorIndexing = true
            // Then have a background job retry failed indexing
        }

        return incident.Id;
    }
}