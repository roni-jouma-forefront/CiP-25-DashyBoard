using DashyBoard.Application.Common.Models;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommand : IRequest<Result<int>>
{
    public int? HotelId { get; set; }
    public int? BookingId { get; set; }
    public string? Content { get; set; }
    public DateTime? ExpiresAt { get; set; }
}