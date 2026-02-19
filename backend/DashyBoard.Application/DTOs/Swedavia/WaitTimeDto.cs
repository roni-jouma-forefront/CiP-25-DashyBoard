namespace DashyBoard.Application.DTOs.Swedavia;

public class WaitTimeDto
{
    public string QueueName { get; set; } = string.Empty;
    public string Terminal { get; set; } = string.Empty;
    public DateTime CurrentTime { get; set; }
    public int CurrentProjectedWaitTime { get; set; }
    public bool IsFastTrack { get; set; }
}
