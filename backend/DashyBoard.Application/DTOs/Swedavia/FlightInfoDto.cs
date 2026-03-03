namespace DashyBoard.Application.DTOs.Swedavia;

public class FlightInfoDto
{
    public string FlightId { get; set; } = string.Empty;
    public string? DepartureAirportIcao { get; set; }
    public string? ArrivalAirportIcao { get; set; }
    public LocationAndStatusDto? LocationAndStatus { get; set; }
    public FlightTimeDto? ArrivalTime { get; set; }
    public FlightTimeDto? DepartureTime { get; set; }
}

public class LocationAndStatusDto
{
    public string? Terminal { get; set; }
    public string? Gate { get; set; }
    public string? FlightLegStatusEnglish { get; set; }
}

public class FlightTimeDto
{
    public DateTime? EstimatedUtc { get; set; }
    public DateTime? ScheduledUtc { get; set; }
}
