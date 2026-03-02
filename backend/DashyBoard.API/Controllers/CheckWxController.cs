using DashyBoard.Application.DTOs.CheckWX;
using DashyBoard.Application.Features.CheckWx.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckWxController : ControllerBase
{
    private readonly IMediator _mediator;

    public CheckWxController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get current weather conditions at an airport
    /// </summary>
    /// <param name="icao">ICAO code of the airport (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Current weather conditions at the specified airport</returns>
    [HttpGet("{icao}")]
    [ProducesResponseType(typeof(IEnumerable<CheckWxDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeather(
        string icao,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(icao))
            return BadRequest("ICAO code is required");

        try
        {
            var query = new GetCheckWxQuery(icao);
            var weather = await _mediator.Send(query, cancellationToken);

            return Ok(weather);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Failed to fetch weather data from CheckWX API",
                    error = ex.Message,
                }
            );
        }
    }
}
