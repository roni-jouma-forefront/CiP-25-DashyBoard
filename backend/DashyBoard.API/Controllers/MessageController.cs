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

    [HttpGet]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MessageDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetMessagesForMirrorQuery());
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<int>>> Create([FromBody] CreateMessageCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}