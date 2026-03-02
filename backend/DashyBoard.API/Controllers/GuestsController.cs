using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Commands.CreateGuest;
using DashyBoard.Application.Features.Commands.DeleteGuest;
using DashyBoard.Application.Features.Commands.UpdateGuest;
using DashyBoard.Application.Features.Queries.GetAllGuests;
using DashyBoard.Application.Features.Queries.GetGuest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GuestsController> _logger;

    public GuestsController(IMediator mediator, ILogger<GuestsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Create a new guest
    /// </summary>
    /// <param name="createGuestDto">Guest data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created guest</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GuestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateGuest([FromBody] GuestDto createGuestDto)
    {
        if (
            string.IsNullOrWhiteSpace(createGuestDto.FirstName)
            || string.IsNullOrWhiteSpace(createGuestDto.LastName)
        )
            return BadRequest("First name and last name are required.");

        try
        {
            var command = new CreateGuestCommand(
                FirstName: createGuestDto.FirstName,
                LastName: createGuestDto.LastName
            );
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating guest");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while creating the guest."
            );
        }
    }

    /// <summary>
    /// Get all guests, optionally filtered by first name and/or last name
    /// </summary>
    /// <param name="firstName">Optional first name filter</param>
    /// <param name="lastName">Optional last name filter</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of guests</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<GuestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllGuests(
        string? firstName,
        string? lastName,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var query = new GetAllGuestsQuery(firstName, lastName);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving guests");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving the guests."
            );
        }
    }

    /// <summary>
    /// Get a guest by ID
    /// </summary>
    /// <param name="id">Guest ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Guest details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GuestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGuestById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetGuestQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving guest with ID {GuestId}", id);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving the guest."
            );
        }
    }

    /// <summary>
    /// Update a guest by ID
    /// </summary>
    /// <param name="id">Guest ID</param>
    /// <param name="updateGuestDto">Updated guest data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Updated guest details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GuestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateGuest(
        Guid id,
        [FromBody] GuestDto updateGuestDto,
        CancellationToken cancellationToken
    )
    {
        if (
            string.IsNullOrWhiteSpace(updateGuestDto.FirstName)
            || string.IsNullOrWhiteSpace(updateGuestDto.LastName)
        )
            return BadRequest("First name and last name are required.");

        try
        {
            var command = new UpdateGuestCommand(
                Id: id,
                FirstName: updateGuestDto.FirstName,
                LastName: updateGuestDto.LastName
            );
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating guest with ID {GuestId}", id);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while updating the guest."
            );
        }
    }

    /// <summary>
    /// Delete a guest by ID
    /// </summary>
    /// <param name="id">Guest ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted guest details</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(GuestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteGuest(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteGuestCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting guest with ID {GuestId}", id);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while deleting the guest."
            );
        }
    }
}
