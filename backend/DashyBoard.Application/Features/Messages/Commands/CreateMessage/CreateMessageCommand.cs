using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommand : IRequest<Result<Guid>>
{
    public Guid? HotelId { get; set; }
    public Guid? BookingId { get; set; }
    public string? Content { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
