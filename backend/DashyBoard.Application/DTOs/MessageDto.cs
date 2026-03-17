namespace DashyBoard.Application.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid? HotelId { get; set; }
    public Guid? BookingId { get; set; }
    public string? Content { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
