namespace DashyBoard.Application.DTOs;

public class LoginRequestDto
{
    public Guid HotelId { get; set; }
    public string Password { get; set; } = string.Empty;
}
