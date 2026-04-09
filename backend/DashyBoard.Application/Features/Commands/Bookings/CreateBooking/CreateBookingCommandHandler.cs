using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Bookings.CreateBooking;

public class CreateBookingCommandHandler(IRepository<Booking> repository, IDateTime dateTime)
    : IRequestHandler<CreateBookingCommand, Result<BookingDto>>
{
    public async Task<Result<BookingDto>> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        if (request.RoomId.HasValue)
        {
            var overlapping = await repository.FindAsync(
                b =>
                    b.RoomId == request.RoomId
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
                    b.GuestId == request.GuestId
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

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = request.RoomId,
            GuestId = request.GuestId,
            FlightId = request.FlightId,
            NumberOfGuests = request.NumberOfGuests,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            BookingStatus = request.BookingStatus,
            CreatedAt = dateTime.CetNow,
        };

        await repository.AddAsync(booking, cancellationToken);

        return Result<BookingDto>.Success(
            new BookingDto
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                GuestId = booking.GuestId,
                FlightId = booking.FlightId,
                NumberOfGuests = booking.NumberOfGuests,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                BookingStatus = booking.BookingStatus,
            }
        );
    }
}
