using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Bookings.UpdateBooking;

public class UpdateBookingCommandHandler(IRepository<Booking> repository, IDateTime dateTime)
    : IRequestHandler<UpdateBookingCommand, Result<BookingDto>>
{
    public async Task<Result<BookingDto>> Handle(
        UpdateBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        var booking = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found.");

        if (
            booking.BookingStatus == Booking.Status.Completed
            || booking.BookingStatus == Booking.Status.Cancelled
        )
            return Result<BookingDto>.Failure(
                $"Cannot modify a booking with status '{booking.BookingStatus}'."
            );

        if (request.RoomId.HasValue)
        {
            var overlapping = await repository.FindAsync(
                b =>
                    b.Id != request.Id
                    && b.RoomId == request.RoomId
                    && b.BookingStatus != Booking.Status.Cancelled
                    && b.CheckIn < request.CheckOut
                    && b.CheckOut > request.CheckIn,
                cancellationToken
            );

            if (overlapping.Any())
                return Result<BookingDto>.Failure(
                    "Room is already booked during the requested period."
                );
        }

        if (request.GuestId.HasValue)
        {
            var guestOverlapping = await repository.FindAsync(
                b =>
                    b.Id != request.Id
                    && b.GuestId == request.GuestId
                    && b.BookingStatus != Booking.Status.Cancelled
                    && b.CheckIn < request.CheckOut
                    && b.CheckOut > request.CheckIn,
                cancellationToken
            );

            if (guestOverlapping.Any())
                return Result<BookingDto>.Failure(
                    "Guest already has a booking during the requested period."
                );
        }

        booking.RoomId = request.RoomId;
        booking.GuestId = request.GuestId;
        booking.FlightNumber = request.FlightNumber;
        booking.NumberOfGuests = request.NumberOfGuests;
        booking.CheckIn = request.CheckIn;
        booking.CheckOut = request.CheckOut;
        booking.BookingStatus = request.BookingStatus;
        booking.UpdatedAt = dateTime.CetNow;
        booking.UpdatedBy = "work in progress";

        await repository.UpdateAsync(booking, cancellationToken);

        return Result<BookingDto>.Success(
            new BookingDto
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                GuestId = booking.GuestId,
                FlightNumber = booking.FlightNumber,
                NumberOfGuests = booking.NumberOfGuests,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                BookingStatus = booking.BookingStatus,
            }
        );
    }
}
