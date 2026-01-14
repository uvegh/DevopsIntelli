using Xunit;
using DevopsIntelli.Domain.Common.Entities; // Assuming this namespace based on the image
using DevopsIntelli.Domain.Common.Enums;
using DevopsIntelli.Domain.Common.Enum;   // Assuming an Enums folder exists

public class IncidentTests
{
    [Fact]
    public void CreateNew_ValidData_ShouldInitializeCorrectly()
    {
        // Arrange
        var title = "Disk Space Low";
        var description = "The primary production server is running out of space.";
        var severity = Severity.High;
        var method = DetectionMethod.HealthCheck;
        var detectedBy = "Monitoring System";

        // Act
        //  hypothetical factory method defined above
        var incident = Incident.Create (title, description, severity, method, detectedBy);

        // Assert
        // Check that the properties match the input data
        Assert.Equal(title, incident.Title );
        Assert.Equal(description, incident.Description );
        Assert.Equal(severity, incident.Severity );
        Assert.Equal(method, incident.DetectionMethod );
        Assert.Equal(detectedBy, incident.DetectedBy );

        // Check that non-nullable properties have default states or are null initially
        Assert.Equal(IncidentStatus.Open, incident.Status ); // Check the default status
        Assert.Null(incident.ResolvedAt ); // Should be null until resolved
        Assert.Null(incident.Analysis ); // Should be null initially
    }

    [Fact]
    public void CreateNew_EmptyTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var description = "Test description";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            Incident.Create("", description, Severity.Medium, DetectionMethod.Manual, "User")
        );

        // Optional: Verify the exception message contains expected text
        Assert.Contains("Title cannot be empty.", exception.Message );
    }
}
