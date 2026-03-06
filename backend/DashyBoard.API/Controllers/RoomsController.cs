using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Commands.CreateRoom;
using DashyBoard.Application.Features.Commands.DeleteRoom;
using DashyBoard.Application.Features.Commands.UpdateRoom;
using DashyBoard.Application.Features.Queries.GetAllRooms;
using DashyBoard.Application.Features.Queries.GetRoom;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    public sealed class CreateRoomRequest
    {
        public Guid HotelId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
    }

    public sealed class UpdateRoomRequest
    {
        public string RoomNumber { get; set; } = string.Empty;
    }

    private readonly IMediator _mediator;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(IMediator mediator, ILogger<RoomsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Create a new room
    /// </summary>
    /// <param name="createRoomRequest">Room data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created room</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRoom(
        [FromBody] CreateRoomRequest createRoomRequest,
        CancellationToken cancellationToken
    )
    {
        if (
            createRoomRequest.HotelId == Guid.Empty
            || string.IsNullOrWhiteSpace(createRoomRequest.RoomNumber)
        )
            return BadRequest("Hotel ID and room number are required.");

        try
        {
            var command = new CreateRoomCommand(
                HotelId: createRoomRequest.HotelId,
                RoomNumber: createRoomRequest.RoomNumber
            );
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating room");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while creating the room."
            );
        }
    }

    /// <summary>
    /// Get all rooms for a hotel
    /// </summary>
    /// <param name="hotelId">Hotel ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of rooms</returns>
    [HttpGet("hotel/{hotelId}")]
    [ProducesResponseType(typeof(List<RoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllRoomsByHotelId(
        Guid hotelId,
        CancellationToken cancellationToken
    )
    {
        if (hotelId == Guid.Empty)
            return BadRequest("Hotel ID is required.");

        try
        {
            var query = new GetAllRoomsQuery(hotelId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while retrieving rooms for hotel ID {HotelId}",
                hotelId
            );
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving rooms."
            );
        }
    }

    /// <summary>
    /// Get one room by hotel ID and room ID
    /// </summary>
    /// <param name="hotelId">Hotel ID</param>
    /// <param name="roomId">Room ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Room details</returns>
    [HttpGet("hotel/{hotelId}/{roomId}")]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoomByHotelAndRoomId(
        Guid hotelId,
        Guid roomId,
        CancellationToken cancellationToken
    )
    {
        if (hotelId == Guid.Empty || roomId == Guid.Empty)
            return BadRequest("Hotel ID and room ID are required.");

        try
        {
            var query = new GetRoomQuery(hotelId, roomId);
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
                return NotFound($"Room with ID '{roomId}' was not found for hotel '{hotelId}'.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while retrieving room {RoomId} for hotel ID {HotelId}",
                roomId,
                hotelId
            );
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving the room."
            );
        }
    }

    /// <summary>
    /// Update one room by hotel ID and room ID
    /// </summary>
    /// <param name="hotelId">Hotel ID</param>
    /// <param name="roomId">Room ID</param>
    /// <param name="updateRoomRequest">Updated room data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Updated room details</returns>
    [HttpPut("hotel/{hotelId}/{roomId}")]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRoom(
        Guid hotelId,
        Guid roomId,
        [FromBody] UpdateRoomRequest updateRoomRequest,
        CancellationToken cancellationToken
    )
    {
        if (hotelId == Guid.Empty || roomId == Guid.Empty)
            return BadRequest("Hotel ID and room ID are required.");

        if (string.IsNullOrWhiteSpace(updateRoomRequest.RoomNumber))
            return BadRequest("Room number is required.");

        try
        {
            var command = new UpdateRoomCommand(
                Id: roomId,
                HotelId: hotelId,
                RoomNumber: updateRoomRequest.RoomNumber
            );
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Succeeded)
                return NotFound(result.Errors.FirstOrDefault() ?? "Room not found.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while updating room {RoomId} for hotel ID {HotelId}",
                roomId,
                hotelId
            );
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while updating the room."
            );
        }
    }

    /// <summary>
    /// Delete one room by hotel ID and room ID
    /// </summary>
    /// <param name="hotelId">Hotel ID</param>
    /// <param name="roomId">Room ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted room ID</returns>
    [HttpDelete("hotel/{hotelId}/{roomId}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRoom(
        Guid hotelId,
        Guid roomId,
        CancellationToken cancellationToken
    )
    {
        if (hotelId == Guid.Empty || roomId == Guid.Empty)
            return BadRequest("Hotel ID and room ID are required.");

        try
        {
            var command = new DeleteRoomCommand(roomId, hotelId);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Succeeded)
                return NotFound(result.Errors.FirstOrDefault() ?? "Room not found.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while deleting room {RoomId} for hotel ID {HotelId}",
                roomId,
                hotelId
            );
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while deleting the room."
            );
        }
    }
}
