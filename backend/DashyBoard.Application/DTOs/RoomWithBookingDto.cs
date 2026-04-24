using DashyBoard.Domain.Entities;

namespace DashyBoard.Application.DTOs;

/// <summary>
/// Enriched room DTO that includes active booking and guest information.
/// </summary>
public class RoomWithBookingDto
{
    public Guid Id { get; set; }
    public required Guid HotelId { get; set; }
    public required string RoomNumber { get; set; }
    public BookingWithGuestDto? ActiveBooking { get; set; }
}

/// <summary>
/// Enriched booking DTO that includes guest information.
/// </summary>
public class BookingWithGuestDto
{
    public Guid Id { get; set; }
    public Guid? RoomId { get; set; }
    public Guid? FlightId { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public Booking.Status BookingStatus { get; set; }
    public GuestInfoDto? Guest { get; set; }
}

/// <summary>
/// Simplified guest info for embedding in other DTOs.
/// </summary>
public class GuestInfoDto
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
