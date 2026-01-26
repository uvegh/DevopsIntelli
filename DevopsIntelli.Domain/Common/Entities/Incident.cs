using DevopsIntelli.Domain.Common.Enums;
using DevopsIntelli.Domain.Events;


namespace DevopsIntelli.Domain.Common.Entities;

public class Incident : BaseEntity
{
    private Incident()
    {
        DetectedBy = string.Empty;
        RemdiationSteps = new List<string>();
        AffectedService = string.Empty;
    }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Severity Severity { get; private set; }
    public IncidentStatus Status { get; private set; }
    public DetectionMethod DetectionMethod { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public string DetectedBy { get; private set; }
    public string? Analysis { get; private set; }
    public double? AnalysisConfidence { get; private set; }
    public List<string> RemdiationSteps { get; private set; }
    public string AffectedService { get; private set; }
    public DateTime DetectedAt { get; private set; }
   
    

    //public Incident(string title, string description, Severity severity, IncidentStatus incidentStatus, DetectionMethod detectionMethod, string detectedBy)
    //{
    //    Title = title;
    //    Description = description;
    //    Severity = severity;
    //    Status = incidentStatus;
    //    DetectionMethod = detectionMethod;
    //    DetectedBy = detectedBy;
    //}

    public static Incident Create(string title, string tenantId, string description, Severity severity, IncidentStatus status,  DetectionMethod detectionMethod,string detectedBy, string affectedService)
    {
        if (string.IsNullOrWhiteSpace(tenantId))
            throw new ArgumentException("Tenant Id is required", nameof(tenantId));


        if (string.IsNullOrWhiteSpace(title))
        
            throw new ArgumentException("Title is required", nameof(title));

        var newIncident = new Incident
        {
            Id = Guid.NewGuid(),

            Title = title,
            TenantId = tenantId,
            Description = description,
            Severity = severity,
            Status = IncidentStatus.Open,
            DetectionMethod = detectionMethod,
            DetectedBy = detectedBy,
            AffectedService=affectedService
            

        };
        newIncident.RaiseDomainEvent(new IncidentDetectedEvent
        {
            TenantId = tenantId,
            IncidentId = newIncident.Id,
            Severity= severity,
            AffectedService=affectedService
           
        });
            return newIncident;

        }

    public  void AddAnalysis(double analysisConfidence, string analysis, string
        tenantId)
    {
        if (analysisConfidence < 0 || analysisConfidence > 1)
            throw new ArgumentException("Analysis must be in between 0 to 1 ", nameof(analysisConfidence));
        Analysis = analysis;
        AnalysisConfidence = analysisConfidence;
        UpdatedAt = DateTime.UtcNow;

        var incidentAnalyzedEvent = new IncidentAnalyzedEvent
        {
            OccuredAt = DateTime.UtcNow,
            EventId=Guid.NewGuid(),
            TenantId= tenantId,
            Confidence=analysisConfidence

        };
        RaiseDomainEvent(incidentAnalyzedEvent);
    }

    public void IncidentResolved()
    {
        Status = IncidentStatus.Resolved;
        var incidentResolved = new IncidentResolvedEvent
        {
            EventId = Guid.NewGuid(),
            OccuredAt = DateTime.UtcNow,
            
            
        };



    }
    }

