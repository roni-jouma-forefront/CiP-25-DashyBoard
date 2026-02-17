using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class Message : BaseAuditableEntity
{
    public int Id { get; set; }
    public int? HotelId { get; set; }
    public int? BookingId { get; set; }
    public string? Content { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
}
