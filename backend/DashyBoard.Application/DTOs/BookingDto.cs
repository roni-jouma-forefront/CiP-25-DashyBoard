using DashyBoard.Domain.Entities;

namespace DashyBoard.Application.DTOs;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid? RoomId { get; set; }
    public Guid? GuestId { get; set; }
    public string FlightNumber { get; set; } = null!;
    public int NumberOfGuests { get; set; }
    public DateTime CheckIn { get; set; } = DateTime.UtcNow;
    public DateTime CheckOut { get; set; } = DateTime.UtcNow.AddDays(1);
    public Booking.Status BookingStatus { get; set; }
}
