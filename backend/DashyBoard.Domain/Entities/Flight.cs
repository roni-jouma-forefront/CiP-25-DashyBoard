using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class Flight : BaseAuditableEntity
{
    public int Id { get; set; }
    public int? FlightNumber { get; set; }
    public string? Gate { get; set; }
    public string? Status { get; set; }
    public DateTime? ScheduledDeparture { get; set; }
    public DateTime? ScheduledArrival { get; set; }
    public enum FlightType
    {
        Arrival,
        Departure
    }
}
