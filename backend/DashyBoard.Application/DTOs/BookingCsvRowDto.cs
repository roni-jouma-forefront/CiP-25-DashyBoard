namespace DashyBoard.Application.DTOs;

public class BookingCsvRowDto
{
    public required string GuestFirstName { get; set; }
    public required string GuestLastName { get; set; }
    public required string RoomNumber { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IsPilot { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public required string BookingStatus { get; set; }
    public required string FlightNumber { get; set; }
    public required string FlightType { get; set; }
}

public class CsvImportResultDto
{
    public int TotalRows { get; set; }
    public int SuccessfulImports { get; set; }
    public int FailedImports { get; set; }
    public List<string> Errors { get; set; } = [];
    public List<Guid> CreatedBookingIds { get; set; } = [];
}
