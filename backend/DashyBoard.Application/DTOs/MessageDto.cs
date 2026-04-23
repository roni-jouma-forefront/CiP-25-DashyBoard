namespace DashyBoard.Application.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid? HotelId { get; set; }
    public Guid? BookingId { get; set; }
    public string? PostedBy { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime PostAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public string? RecurrenceType { get; set; }
    public string? RecurrenceDays { get; set; }
    public TimeOnly? RecurrenceTimeStart { get; set; }
    public TimeOnly? RecurrenceTimeEnd { get; set; }
    public DateTime CreatedAt { get; set; }
}
