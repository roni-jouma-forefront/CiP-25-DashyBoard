using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommand : IRequest<Result<Guid>>
{
    public Guid? HotelId { get; set; }
    public Guid? BookingId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? PostedBy { get; set; }
    public DateTime? PostAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string RecurrenceType { get; set; } = "None";
    public string? RecurrenceDays { get; set; }
    public TimeOnly? RecurrenceTimeStart { get; set; }
    public TimeOnly? RecurrenceTimeEnd { get; set; }
}
