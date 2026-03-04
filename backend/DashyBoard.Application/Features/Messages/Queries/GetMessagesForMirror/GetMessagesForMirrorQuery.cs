using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Queries.GetMessagesForMirror;

public class GetMessagesForMirrorQuery : IRequest<List<MessageDto>>
{
    public Guid? HotelId { get; set; }
    public Guid? BookingId { get; set; }
}
