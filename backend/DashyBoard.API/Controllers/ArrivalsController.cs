using DashyBoard.Application.DTOs.Swedavia;
using DashyBoard.Application.Features.Flights.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArrivalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArrivalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get flight arrivals for today at a specific airport
    /// </summary>
    /// <param name="airport">Airport IATA code (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of flight arrivals</returns>
    [HttpGet("airport/{airport}")]
    [ProducesResponseType(typeof(IEnumerable<FlightInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetArrivals(
        string airport,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(airport) || airport.Length != 3)
            return BadRequest("Airport code must be a valid 3-letter IATA code");

        try
        {
            var query = new GetArrivalsQuery(airport, DateOnly.FromDateTime(DateTime.UtcNow));
            var arrivals = await _mediator.Send(query, cancellationToken);

            return Ok(arrivals);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = "Failed to fetch arrivals from Swedavia API", error = ex.Message }
            );
        }
    }

    /// <summary>
    /// Get flight arrivals for a specific date at a specific airport
    /// </summary>
    /// <param name="airport">Airport IATA code (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="date">Date in format yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of flight arrivals</returns>
    [HttpGet("airport/{airport}/{date}")]
    [ProducesResponseType(typeof(IEnumerable<FlightInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetArrivalsByDate(
        string airport,
        string date,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(airport) || airport.Length != 3)
            return BadRequest("Airport code must be a valid 3-letter IATA code");

        if (!DateOnly.TryParse(date, out var parsedDate))
            return BadRequest("Invalid date format. Use yyyy-MM-dd");

        try
        {
            var query = new GetArrivalsQuery(airport, parsedDate);
            var arrivals = await _mediator.Send(query, cancellationToken);

            return Ok(arrivals);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = "Failed to fetch arrivals from Swedavia API", error = ex.Message }
            );
        }
    }

    /// <summary>
    /// Get flight arrivals for a specific flight ID at an airport
    /// </summary>
    /// <param name="airport">Airport IATA code (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="flightId">Flight ID (e.g., SK536)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Flight arrival information</returns>
    [HttpGet("airport/{airport}/flight/{flightId}")]
    [ProducesResponseType(typeof(IEnumerable<FlightInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetArrivalsByFlightId(
        string airport,
        string flightId,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(airport) || airport.Length != 3)
            return BadRequest("Airport code must be a valid 3-letter IATA code");

        if (string.IsNullOrWhiteSpace(flightId))
            return BadRequest("Flight ID is required");

        try
        {
            var query = new GetArrivalsQuery(
                airport,
                DateOnly.FromDateTime(DateTime.UtcNow),
                flightId
            );
            var arrivals = await _mediator.Send(query, cancellationToken);

            return Ok(arrivals);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = "Failed to fetch arrivals from Swedavia API", error = ex.Message }
            );
        }
    }
}
