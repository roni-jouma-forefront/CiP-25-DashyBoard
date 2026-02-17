using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class Guest : BaseAuditableEntity
{
    public int Id { get; set; }
    public required string FullName { get; set; }
}
