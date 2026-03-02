using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class Guest : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
