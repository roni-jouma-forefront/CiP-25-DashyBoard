using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Examples.Commands;
using DashyBoard.Application.Features.Examples.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamplesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExamplesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ExampleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ExampleDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetExamplesQuery());
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreateExampleCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
