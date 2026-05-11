using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Commands.Hotels.CreateHotel;
using DashyBoard.Application.Features.Commands.Hotels.DeleteHotel;
using DashyBoard.Application.Features.Commands.Hotels.UpdateHotel;
using DashyBoard.Application.Features.Queries.Hotels.GetAllHotels;
using DashyBoard.Application.Features.Queries.Hotels.GetHotel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    public sealed class CreateHotelRequest
    {
        public string Name { get; set; } = string.Empty;
        public string IcaoCode { get; set; } = string.Empty;
    }

    public sealed class UpdateHotelRequest
    {
        public string Name { get; set; } = string.Empty;
        public string IcaoCode { get; set; } = string.Empty;
    }

    private readonly IMediator _mediator;
    private readonly ILogger<HotelsController> _logger;

    public HotelsController(IMediator mediator, ILogger<HotelsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Create a new hotel
    /// </summary>
    /// <param name="createHotelRequest">Hotel data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created hotel</returns>
    [HttpPost]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateHotel(
        [FromBody] CreateHotelRequest createHotelRequest,
        CancellationToken cancellationToken
    )
    {
        if (
            string.IsNullOrWhiteSpace(createHotelRequest.Name)
            || string.IsNullOrWhiteSpace(createHotelRequest.IcaoCode)
        )
            return BadRequest("Hotel name and ICAO code are required.");

        try
        {
            var command = new CreateHotelCommand(
                Name: createHotelRequest.Name,
                IcaoCode: createHotelRequest.IcaoCode
            );
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while creating hotel with name {HotelName}",
                createHotelRequest.Name
            );
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while creating the hotel."
            );
        }
    }

    /// <summary>
    /// Get all hotels
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of hotels</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<HotelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllHotels(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetAllHotelsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving hotels");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving hotels."
            );
        }
    }

    /// <summary>
    /// Get one hotel by hotel ID
    /// </summary>
    /// <param name="hotelId">Hotel ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Hotel details</returns>
    [HttpGet("hotel/{hotelId}")]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHotelById(Guid hotelId, CancellationToken cancellationToken)
    {
        if (hotelId == Guid.Empty)
            return BadRequest("Hotel ID is required.");

        try
        {
            var query = new GetHotelQuery(hotelId);
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
                return NotFound($"Hotel with ID '{hotelId}' was not found.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while retrieving hotel with ID {HotelId}",
                hotelId
            );
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving the hotel."
            );
        }
    }

    /// <summary>
    /// Update one hotel by hotel ID
    /// </summary>
    /// <param name="hotelId">Hotel ID</param>
    /// <param name="updateHotelRequest">Updated hotel data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Updated hotel details</returns>
    [HttpPut("hotel/{hotelId}")]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateHotel(
        Guid hotelId,
        [FromBody] UpdateHotelRequest updateHotelRequest,
        CancellationToken cancellationToken
    )
    {
        if (hotelId == Guid.Empty)
            return BadRequest("Hotel ID is required.");

        if (string.IsNullOrWhiteSpace(updateHotelRequest.Name))
            return BadRequest("Hotel name is required.");

        try
        {
            var command = new UpdateHotelCommand(
                Id: hotelId,
                Name: updateHotelRequest.Name,
                IcaoCode: updateHotelRequest.IcaoCode
            );
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Succeeded)
                return NotFound(result.Errors.FirstOrDefault() ?? "Hotel not found.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating hotel with ID {HotelId}", hotelId);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while updating the hotel."
            );
        }
    }

    /// <summary>
    /// Delete one hotel by hotel ID
    /// </summary>
    /// <param name="hotelId">Hotel ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted hotel ID</returns>
    [HttpDelete("hotel/{hotelId}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteHotel(Guid hotelId, CancellationToken cancellationToken)
    {
        if (hotelId == Guid.Empty)
            return BadRequest("Hotel ID is required.");

        try
        {
            var command = new DeleteHotelCommand(hotelId);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Succeeded)
                return NotFound(result.Errors.FirstOrDefault() ?? "Hotel not found.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting hotel with ID {HotelId}", hotelId);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while deleting the hotel."
            );
        }
    }
}
