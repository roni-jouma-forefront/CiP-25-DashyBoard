using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Messages.Queries.GetMessagesForRoom;

public class GetMessagesForRoomQuery : IRequest<List<MessageDto>>
{
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
}
