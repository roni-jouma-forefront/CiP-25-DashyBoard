using DashyBoard.Application.DTOs.Swedavia;
using DashyBoard.Application.Features.WaitTimes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WaitTimesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WaitTimesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get current security queue wait times at an airport
    /// </summary>
    /// <param name="airport">Airport code (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of current security queue wait times by terminal</returns>
    [HttpGet("{airport}")]
    [ProducesResponseType(typeof(IEnumerable<WaitTimeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWaitTimes(
        string airport,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(airport))
            return BadRequest("Airport code is required");

        try
        {
            var query = new GetWaitTimesQuery(airport, DateOnly.FromDateTime(DateTime.UtcNow));
            var waitTimes = await _mediator.Send(query, cancellationToken);

            return Ok(waitTimes);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = "Failed to fetch wait times from Swedavia API", error = ex.Message }
            );
        }
    }
}
