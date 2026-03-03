using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Queries.GetMessagesForMirror;

public class GetMessagesForMirrorQuery : IRequest<List<MessageDto>>
{
    public int? HotelId { get; set; }
    public int? BookingId { get; set; }
}