namespace DashyBoard.Domain.Entities.ExternalEntities;

public class FlightInfoResponse
{
    public AirportInfoApiModel? To { get; set; } // For arrivals
    public AirportInfoApiModel? From { get; set; } // For departures
    public List<FlightInfoApiModel>? Flights { get; set; }
}

public class AirportInfoApiModel
{
    public string? ArrivalAirportIcao { get; set; }
    public string? DepartureAirportIcao { get; set; }
}

public class FlightInfoApiModel
{
    public string? FlightId { get; set; }
    public LocationAndStatusApiModel? LocationAndStatus { get; set; }
    public FlightTimeApiModel? ArrivalTime { get; set; } // For arrivals
    public FlightTimeApiModel? DepartureTime { get; set; } // For departures
    public FlightLegIdentifierApiModel? FlightLegIdentifier { get; set; }
}

public class FlightLegIdentifierApiModel
{
    public string? DepartureAirportIcao { get; set; }
    public string? ArrivalAirportIcao { get; set; }
}

public class LocationAndStatusApiModel
{
    public string? Terminal { get; set; }
    public string? Gate { get; set; }
    public string? FlightLegStatusEnglish { get; set; }
}

public class FlightTimeApiModel
{
    public DateTime? EstimatedUtc { get; set; }
    public DateTime? ScheduledUtc { get; set; }
}
