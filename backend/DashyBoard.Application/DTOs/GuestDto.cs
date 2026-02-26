namespace DashyBoard.Application.DTOs;

public class GuestDto
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

}
