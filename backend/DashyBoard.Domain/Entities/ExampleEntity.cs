using DashyBoard.Domain.Common;

namespace DashyBoard.Domain.Entities;

public class ExampleEntity : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
