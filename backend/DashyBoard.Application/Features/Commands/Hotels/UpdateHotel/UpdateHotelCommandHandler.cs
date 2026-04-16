using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Hotels.UpdateHotel;

public class UpdateHotelCommandHandler(IRepository<Hotel> repository, IDateTime dateTime)
    : IRequestHandler<UpdateHotelCommand, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(
        UpdateHotelCommand request,
        CancellationToken cancellationToken
    )
    {
        var hotel = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (hotel == null)
        {
            return Result<HotelDto>.Failure("Hotel not found.");
        }

        hotel.Name = request.Name;
        hotel.IcaoCode = request.IcaoCode;
        hotel.UpdatedAt = dateTime.CetNow;
        hotel.UpdatedBy = "work in progress";

        await repository.UpdateAsync(hotel, cancellationToken);

        return Result<HotelDto>.Success(
            new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                IcaoCode = hotel.IcaoCode,
            }
        );
    }
}
