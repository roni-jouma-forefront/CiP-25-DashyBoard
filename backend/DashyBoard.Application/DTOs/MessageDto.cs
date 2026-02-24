namespace DashyBoard.Application.DTOs;

public class MessageDto
{
    public int Id { get; set; }
    public int? HotelId { get; set; }
    public int? BookingId { get; set; }
    public string? Content { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
