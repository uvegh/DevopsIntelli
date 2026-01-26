
using DevopsIntelli.Domain.Common.Entities;
using DevopsIntelli.Domain.Common.Enums;
using System.Net.NetworkInformation;
using Xunit;

public class IncidentTest
{
    private readonly string _title = "DB connection timeout";
    private readonly string _description = "connection timeout on application running";
    private readonly string _tenantId = "tenant-123";
    private readonly Severity _severity = Severity.High;
    private readonly IncidentStatus _status = IncidentStatus.Open;
    private readonly string _detectedBy = "monitoring systems";
    private readonly DetectionMethod _detectionMethod = DetectionMethod.AnomalyDetection;
    private readonly string _affectedService = "Orderservice";
    private readonly ITestOutputHelper _output;

    public IncidentTest(ITestOutputHelper output)
    {
        _output = output;
    }

    private Incident CreateValidIncident(string title, string tenantId)
    {
     return   Incident.Create(
            title,

tenantId,
_description,
_severity,
_status,
_detectionMethod,
_detectedBy,
_affectedService


            );

         

    }

    //- this marks it as a test
    [Fact]
    //MethodName_Scenario_ExpectedResult()  - test method
    public void Create_Valid_Data_Should_InitializeCorrectly()
    {
        //arrange, this is basically simulating the data that'll be tested creating a real like scenario


        //ACT -execute method to test
        var newIncident = CreateValidIncident(_title, _tenantId);

        //Assert - Verify everything is Correct
        Assert.NotNull(newIncident);// check new iuncident was created
        Assert.NotEqual(Guid.Empty, newIncident.Id);// check if its not empty,hence created
        Assert.Equal(_title, newIncident.Title); //title should be equal to created title
        Assert.Equal(_description, newIncident.Description);
        Assert.Equal(_tenantId, newIncident.TenantId);
        Assert.Equal(_severity, newIncident.Severity);
        Assert.Equal(_detectionMethod, newIncident.DetectionMethod);
        Assert.Equal(_detectedBy, newIncident.DetectedBy);
        Assert.Equal(_affectedService, newIncident.AffectedService);


        //verify default state

        //Assert.Equal( , newIncident.RemdiationSteps);

        //verify default status
        Assert.Equal(IncidentStatus.Open, newIncident.Status);

        //check if nullable vals are null by default
        Assert.Null(newIncident.AnalysisConfidence);
        Assert.Null(newIncident.Analysis);
        Assert.Null(newIncident.ResolvedAt);

        //Verify if collection types were initialized
        Assert.NotNull(newIncident.RemdiationSteps);
        Assert.Empty(newIncident.RemdiationSteps);


    }

    //Test multiple invalid using theory,tests for multiple scenariiow
    [Theory]
    [InlineData("")]//empty string
    [InlineData(null)]//test for null
    [InlineData("  ")]//test for whitespace
    public void Create_InvalidTenantId_ShouldThrowArgumentException(string invalidTenantId)
    {

        //Assert 
        Assert.Throws<ArgumentException>(() =>
        {
            var newIncident = CreateValidIncident(_title, invalidTenantId);

        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_InvalidTitle_Should_Throw_Argument(string invalidTitle)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var newIncident =  CreateValidIncident(invalidTitle, _tenantId);

        });

    }

    [Theory]
    [InlineData(10.0)]
    [InlineData(0.9)]

    public void AddAnalysis_InvalidConfidence_ShouldThrow_Argument(double analysisConfidence)
    {
        var incident = CreateValidIncident(_title, _tenantId);
   

        Assert.Throws<ArgumentException>(() =>
        {
            _output.WriteLine("Confidence value being tested {analysisConfidence}", analysisConfidence);
            incident.AddAnalysis(analysisConfidence, "this error is mostly caused by arguments passed outside barrier of 0 - 1", _tenantId);

        });
    }
}