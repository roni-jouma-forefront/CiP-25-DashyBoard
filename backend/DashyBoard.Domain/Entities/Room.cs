using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class Room : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public int HotelId { get; set; }
    public string? RoomNumber { get; set; }
}
