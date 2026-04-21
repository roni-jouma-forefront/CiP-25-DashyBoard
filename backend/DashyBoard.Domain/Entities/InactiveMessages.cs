using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class InactiveMessage : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public Guid? HotelId { get; set; }
    public Guid? BookingId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime ExpiresAt { get; set; }
}
