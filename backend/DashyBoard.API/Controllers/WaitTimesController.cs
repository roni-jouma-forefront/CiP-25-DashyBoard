using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.Swedavia;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

public class WaitTimesController : ControllerBase
{
    private readonly ISwedaviaWaitTimeApiService _swedaviaApiService;

    public WaitTimesController(ISwedaviaWaitTimeApiService swedaviaApiService)
    {
        _swedaviaApiService = swedaviaApiService;
    }
    /// <summary>
    /// Get wait times for all flights at an airport
    /// </summary>
    /// <param name="airport">Airport code (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of wait times</returns>
    [HttpGet("waittimes/{airport}")]
    [ProducesResponseType(typeof(IEnumerable<WaitTimeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWaitTimes(string airport, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(airport))
            return BadRequest("Airport code is required");

        try
        {
            var waitTimes = await _swedaviaApiService.GetWaitTimesAsync(
                airport,
                cancellationToken: cancellationToken);

            return Ok(waitTimes);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Failed to fetch wait times from Swedavia API", error = ex.Message });
        }
    }

    /// <summary>
    /// Get wait times for a specific flight
    /// </summary>
    /// <param name="airport">Airport code (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="flightId">Flight ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Wait times for the specified flight</returns>
    [HttpGet("waittimes/{airport}/flight/{flightId}")]
    [ProducesResponseType(typeof(IEnumerable<WaitTimeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWaitTimesByFlight(
        string airport,
        string flightId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(airport))
            return BadRequest("Airport code is required");

        if (string.IsNullOrWhiteSpace(flightId))
            return BadRequest("Flight ID is required");

        try
        {
            var waitTimes = await _swedaviaApiService.GetWaitTimesAsync(
                airport,
                flightId,
                cancellationToken: cancellationToken);

            return Ok(waitTimes);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Failed to fetch wait times from Swedavia API", error = ex.Message });
        }
    }

    /// <summary>
    /// Get wait times for a specific date
    /// </summary>
    /// <param name="airport">Airport code (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="date">Date in format yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Wait times for the specified date</returns>
    [HttpGet("waittimes/{airport}/date/{date}")]
    [ProducesResponseType(typeof(IEnumerable<WaitTimeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWaitTimesByDate(
        string airport,
        string date,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(airport))
            return BadRequest("Airport code is required");

        if (!DateOnly.TryParse(date, out var parsedDate))
            return BadRequest("Invalid date format. Use yyyy-MM-dd");

        try
        {
            var waitTimes = await _swedaviaApiService.GetWaitTimesAsync(
                airport,
                date: parsedDate,
                cancellationToken: cancellationToken);

            return Ok(waitTimes);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Failed to fetch wait times from Swedavia API", error = ex.Message });
        }
    }
}
