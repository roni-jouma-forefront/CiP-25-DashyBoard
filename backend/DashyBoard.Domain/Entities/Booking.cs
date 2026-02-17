using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class Booking : BaseAuditableEntity
{
    public int Id { get; set; }
    public Guid? RoomId { get; set; }
    public int? GuestId { get; set; }
    public int? DepartureFlightId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public enum Status
    {
        Confirmed,
        Cancelled,
        Completed,
        Active
    }
}
