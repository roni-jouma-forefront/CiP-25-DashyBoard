using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class Booking : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public Guid? RoomId { get; set; }
    public Guid? GuestId { get; set; }
    public int NumberOfGuests { get; set; }
    public Guid? FlightId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public enum Status
    {
        Confirmed,
        Cancelled,
        Completed,
        Active,
    }
}
