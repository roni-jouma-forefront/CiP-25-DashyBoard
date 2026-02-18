using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashyBoard.Domain.Entities.ExternalEntities;

public class FlightInfoResponse
{
    public List<FlightInfoApiModel>? Flights { get; set; }
}

public class FlightInfoApiModel
{
    public string? FlightId { get; set; }
    public LocationAndStatusApiModel? LocationAndStatus { get; set; }
    public ArrivalTimeApiModel? ArrivalTime { get; set; }
}


public class LocationAndStatusApiModel
{
    public string? Gate { get; set; }
    public string? FlightLegStatusEnglish { get; set; }
}

public class ArrivalTimeApiModel
{
    public DateTime? EstimatedUtc { get; set; }
}