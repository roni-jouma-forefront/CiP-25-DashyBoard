using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Messages.Commands.CreateMessage;
using DashyBoard.Application.Features.Messages.Queries.GetMessagesForMirror;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> CreateMessage([FromBody] CreateMessageCommand command)
    {
        var messageId = await _mediator.Send(command);

        return CreatedAtAction(nameof(CreateMessage), new { id = messageId }, messageId);
    }

    [HttpGet("mirror")]
    public async Task<ActionResult<List<MessageDto>>> GetMessagesForMirror(
        [FromQuery] int? hotelId,
        [FromQuery] int? bookingId
    )
    {
        var query = new GetMessagesForMirrorQuery
        {
            HotelId = hotelId,
            BookingId = bookingId
        };
        var messages = await _mediator.Send(query);
        return Ok(messages);
    }


}
