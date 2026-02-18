using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class Admin : BaseAuditableEntity
{
    public int Id { get; set; }
    public int? HotelId { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? FullName { get; set; }
    public string? Role { get; set; }
}
