using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Commands.CreateGuest;
using DashyBoard.Application.Features.Commands.DeleteGuest;
using DashyBoard.Application.Features.Commands.UpdateGuest;
using DashyBoard.Application.Features.Queries.GetAllGuests;
using DashyBoard.Application.Features.Queries.GetGuest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    private bool IsGuestDataValid(GuestDto guestDto, out string errorMessage)
    {
        if (
            string.IsNullOrWhiteSpace(guestDto.FirstName)
            || string.IsNullOrWhiteSpace(guestDto.LastName)
        )
        {
            errorMessage = "First name and last name are required.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Create a new guest
    /// </summary>
    /// <param name="createGuestDto">Guest data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created guest</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GuestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateGuest(
        [FromBody] GuestDto createGuestDto,
        CancellationToken cancellationToken
    )
    {
        if (!IsGuestDataValid(createGuestDto, out var errorMessage))
            return BadRequest(errorMessage);

        try
        {
            var command = new CreateGuestCommand(
                FirstName: createGuestDto.FirstName,
                LastName: createGuestDto.LastName
            );
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetGuestById), new { id = result.Data!.Id }, result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error in {MethodName} with input {Input}",
                nameof(CreateGuest),
                createGuestDto
            );
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
            _logger.LogError(ex, "Error in {MethodName}", nameof(GetAllGuests));
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGuestById(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Guest ID is required.");

        try
        {
            var query = new GetGuestQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
                return NotFound($"Guest with ID '{id}' was not found.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error in {MethodName} with ID {GuestId}",
                nameof(GetGuestById),
                id
            );
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateGuest(
        Guid id,
        [FromBody] GuestDto updateGuestDto,
        CancellationToken cancellationToken
    )
    {
        if (!IsGuestDataValid(updateGuestDto, out var errorMessage))
            return BadRequest(errorMessage);

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
            _logger.LogError(
                ex,
                "Error in {MethodName} with ID {GuestId}",
                nameof(UpdateGuest),
                id
            );
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteGuest(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Guest ID is required.");

        try
        {
            var command = new DeleteGuestCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return NotFound($"Guest with ID '{id}' was not found.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error in {MethodName} with ID {GuestId}",
                nameof(DeleteGuest),
                id
            );
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while deleting the guest."
            );
        }
    }
}
