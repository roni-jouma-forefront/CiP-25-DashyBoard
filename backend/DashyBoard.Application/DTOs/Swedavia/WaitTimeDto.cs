namespace DashyBoard.Application.DTOs.Swedavia;

public class WaitTimeDto
{
    public string Airport { get; set; } = string.Empty;
    public string? FlightId { get; set; }
    public DateTime? Date { get; set; }
    public int WaitTimeMinutes { get; set; }
}
