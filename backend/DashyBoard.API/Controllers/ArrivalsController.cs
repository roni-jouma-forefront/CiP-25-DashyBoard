using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.Swedavia;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArrivalsController : ControllerBase
{
    private readonly ISwedaviaFlightApiService _swedaviaApiService;

    public ArrivalsController(ISwedaviaFlightApiService swedaviaApiService)
    {
        _swedaviaApiService = swedaviaApiService;
    }

    /// <summary>
    /// Get flight arrivals for today
    /// </summary>
    /// <param name="airport">Airport IATA code (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of flight arrivals</returns>
    [HttpGet("arrivals/{airport}")]
    [ProducesResponseType(typeof(IEnumerable<FlightInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetArrivals(string airport, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(airport) || airport.Length != 3)
            return BadRequest("Airport code must be a valid 3-letter IATA code");

        try
        {
            var arrivals = await _swedaviaApiService.GetArrivalsAsync(
                airport,
                DateOnly.FromDateTime(DateTime.UtcNow),
                cancellationToken);
            
            return Ok(arrivals);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = "Failed to fetch arrivals from Swedavia API", error = ex.Message });
        }
    }

    /// <summary>
    /// Get flight arrivals for a specific date
    /// </summary>
    /// <param name="airport">Airport IATA code (e.g., ARN for Stockholm Arlanda)</param>
    /// <param name="date">Date in format yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of flight arrivals</returns>
    [HttpGet("arrivals/{airport}/{date}")]
    [ProducesResponseType(typeof(IEnumerable<FlightInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetArrivalsByDate(
        string airport, 
        string date, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(airport) || airport.Length != 3)
            return BadRequest("Airport code must be a valid 3-letter IATA code");

        if (!DateOnly.TryParse(date, out var parsedDate))
            return BadRequest("Invalid date format. Use yyyy-MM-dd");

        try
        {
            var arrivals = await _swedaviaApiService.GetArrivalsAsync(
                airport,
                parsedDate,
                cancellationToken);
            
            return Ok(arrivals);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = "Failed to fetch arrivals from Swedavia API", error = ex.Message });
        }
    }

    

    ///// <summary>
    ///// Get combined flight and wait time information
    ///// </summary>
    ///// <param name="airport">Airport IATA code (e.g., ARN for Stockholm Arlanda)</param>
    ///// <param name="cancellationToken"></param>
    ///// <returns>Combined flight arrivals and wait times</returns>
    //[HttpGet("combined/{airport}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //public async Task<IActionResult> GetCombinedFlightInfo(
    //    string airport, 
    //    CancellationToken cancellationToken = default)
    //{
    //    if (string.IsNullOrWhiteSpace(airport) || airport.Length != 3)
    //        return BadRequest("Airport code must be a valid 3-letter IATA code");

    //    try
    //    {
    //        var today = DateOnly.FromDateTime(DateTime.UtcNow);
            
    //        var arrivalsTask = _swedaviaApiService.GetArrivalsAsync(airport, today, cancellationToken);
    //        var waitTimesTask = _swedaviaApiService.GetWaitTimesAsync(airport, cancellationToken: cancellationToken);
            
    //        await Task.WhenAll(arrivalsTask, waitTimesTask);
            
    //        return Ok(new
    //        {
    //            airport,
    //            date = today,
    //            arrivals = await arrivalsTask,
    //            waitTimes = await waitTimesTask
    //        });
    //    }
    //    catch (HttpRequestException ex)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError, 
    //            new { message = "Failed to fetch data from Swedavia API", error = ex.Message });
    //    }
    //}
}
