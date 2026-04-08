using DashyBoard.Domain.Entities;

namespace DashyBoard.Application.DTOs;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid? RoomId { get; set; }
    public Guid? GuestId { get; set; }
    public Guid? FlightId { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public Booking.Status BookingStatus { get; set; }
}
