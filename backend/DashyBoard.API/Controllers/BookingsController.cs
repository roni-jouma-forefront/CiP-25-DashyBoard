using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Commands.Bookings.CreateBooking;
using DashyBoard.Application.Features.Commands.Bookings.DeleteBooking;
using DashyBoard.Application.Features.Commands.Bookings.UpdateBooking;
using DashyBoard.Application.Features.Commands.ImportBookingsFromCsv;
using DashyBoard.Application.Features.Queries.Bookings.GetAllBookings;
using DashyBoard.Application.Features.Queries.Bookings.GetBooking;
using DashyBoard.Application.Features.Queries.Bookings.GetBookingWithGuest;
using DashyBoard.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(IMediator mediator, ILogger<BookingsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Create a new booking
    /// </summary>
    /// <param name="request">Booking data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created booking</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBooking(
        [FromBody] BookingDto bookingDto,
        CancellationToken cancellationToken
    )
    {
        if (bookingDto.NumberOfGuests <= 0)
            return BadRequest("Number of guests must be greater than zero.");

        if (bookingDto.CheckOut <= bookingDto.CheckIn)
            return BadRequest("Check-out date must be after check-in date.");

        try
        {
            var command = new CreateBookingCommand(
                RoomId: bookingDto.RoomId,
                GuestId: bookingDto.GuestId,
                FlightNumber: bookingDto.FlightNumber,
                NumberOfGuests: bookingDto.NumberOfGuests,
                CheckIn: bookingDto.CheckIn,
                CheckOut: bookingDto.CheckOut,
                BookingStatus: bookingDto.BookingStatus
            );
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Succeeded)
                return Conflict(result.Errors);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating booking");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                $"{ex.Message} | {ex.InnerException?.Message}"
            );
        }
    }

    /// <summary>
    /// Get all bookings, optionally filtered by guestId, roomId or status
    /// </summary>
    /// <param name="guestId">Optional guest ID filter</param>
    /// <param name="roomId">Optional room ID filter</param>
    /// <param name="bookingStatus">Optional status filter</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of bookings</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllBookings(
        Guid? guestId,
        Guid? roomId,
        Booking.Status? bookingStatus,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var query = new GetAllBookingsQuery(guestId, roomId, bookingStatus);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bookings");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving the bookings."
            );
        }
    }

    /// <summary>
    /// Get a booking by ID
    /// </summary>
    /// <param name="id">Booking ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Booking details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBookingById(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Booking ID is required.");

        try
        {
            var query = new GetBookingQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
                return NotFound($"Booking with ID '{id}' was not found.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving booking with ID {BookingId}", id);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving the booking."
            );
        }
    }

    /// <summary>
    /// Update a booking by ID
    /// </summary>
    /// <param name="id">Booking ID</param>
    /// <param name="request">Updated booking data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Updated booking details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBooking(
        Guid id,
        [FromBody] BookingDto bookingDto,
        CancellationToken cancellationToken
    )
    {
        if (id == Guid.Empty)
            return BadRequest("Booking ID is required.");

        if (bookingDto.NumberOfGuests <= 0)
            return BadRequest("Number of guests must be greater than zero.");

        if (bookingDto.CheckOut <= bookingDto.CheckIn)
            return BadRequest("Check-out date must be after check-in date.");

        try
        {
            var command = new UpdateBookingCommand(
                Id: id,
                RoomId: bookingDto.RoomId,
                GuestId: bookingDto.GuestId,
                FlightNumber: bookingDto.FlightNumber,
                NumberOfGuests: bookingDto.NumberOfGuests,
                CheckIn: bookingDto.CheckIn,
                CheckOut: bookingDto.CheckOut,
                BookingStatus: bookingDto.BookingStatus
            );
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Contains("not found")))
                    return NotFound(result.Errors);
                return Conflict(result.Errors);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating booking with ID {BookingId}", id);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while updating the booking."
            );
        }
    }

    /// <summary>
    /// Delete a booking by ID
    /// </summary>
    /// <param name="id">Booking ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted booking ID</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBooking(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("Booking ID is required.");

        try
        {
            var command = new DeleteBookingCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Succeeded)
                return NotFound(result.Errors);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting booking with ID {BookingId}", id);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while deleting the booking."
            );
        }
    }

    /// <summary>
    /// Import bookings from a CSV file.
    /// </summary>
    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(CsvImportResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportBookingsFromCsv(
        IFormFile file,
        CancellationToken cancellationToken
    )
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded or file is empty.");

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("File must be a CSV file.");

        using var stream = file.OpenReadStream();
        var command = new ImportBookingsFromCsvCommand(stream);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        _logger.LogInformation(
            "CSV import completed: {Total} total, {Success} succeeded, {Failed} failed",
            result.Data!.TotalRows,
            result.Data.SuccessfulImports,
            result.Data.FailedImports
        );

        return Ok(result.Data);
    }

    /// <summary>
    /// Get active booking for a room with guest info.
    /// Returns booking + guest in one call (for mirror display).
    /// </summary>
    /// <param name="roomId">Room ID</param>
    /// <param name="bookingStatus">Optional status filter (defaults to Active)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Booking with guest info, or null if no booking found</returns>
    [HttpGet("room/{roomId}/with-guest")]
    [ProducesResponseType(typeof(BookingWithGuestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBookingWithGuest(
        Guid roomId,
        Booking.Status? bookingStatus,
        CancellationToken cancellationToken
    )
    {
        if (roomId == Guid.Empty)
            return BadRequest("Room ID is required.");

        try
        {
            var query = new GetBookingWithGuestQuery(roomId, bookingStatus);
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
                return NoContent();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while retrieving booking with guest for room ID {RoomId}",
                roomId
            );
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving the booking."
            );
        }
    }
}
