namespace DashyBoard.Application.DTOs.Swedavia;

public class FlightInfoDto
{
    public string FlightId { get; set; } = string.Empty;
    public LocationAndStatusDto? LocationAndStatus { get; set; }
    public ArrivalTimeDto? ArrivalTime { get; set; }
}

public class LocationAndStatusDto
{
    public string? Gate { get; set; }
    public string? FlightLegStatusEnglish { get; set; }
}

public class ArrivalTimeDto
{
    public DateTime? EstimatedUtc { get; set; }
}
