using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class Hotel : BaseAuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IcaoCode { get; set; }
}
