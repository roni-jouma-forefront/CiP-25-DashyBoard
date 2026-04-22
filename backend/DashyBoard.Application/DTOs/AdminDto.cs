namespace DashyBoard.Application.DTOs;

public class AdminDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Role { get; set; }
    public Guid? HotelId { get; set; }
}
