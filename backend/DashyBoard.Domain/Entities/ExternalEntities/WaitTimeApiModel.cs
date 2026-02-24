namespace DashyBoard.Domain.Entities.ExternalEntities;

public class AirportWaitTimeResponse
{
    public int ActiveMeasurementStations { get; set; }
    public List<SecurityQueueWaitTime>? WaitTimes { get; set; }
}

public class SecurityQueueWaitTime
{
    public int Id { get; set; }
    public string? QueueName { get; set; }
    public DateTime CurrentTime { get; set; }
    public int CurrentProjectedWaitTime { get; set; }
    public bool IsFastTrack { get; set; }
    public string? Terminal { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool Overflow { get; set; }
}