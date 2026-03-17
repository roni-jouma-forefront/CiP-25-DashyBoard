namespace DashyBoard.Application.DTOs;

public class RoomDto
{
    public Guid Id { get; set; }
    public required Guid HotelId { get; set; }
    public required string RoomNumber { get; set; }
}
