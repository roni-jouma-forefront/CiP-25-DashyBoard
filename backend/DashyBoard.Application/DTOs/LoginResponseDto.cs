namespace DashyBoard.Application.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public AdminDto Admin { get; set; } = null!;
}
