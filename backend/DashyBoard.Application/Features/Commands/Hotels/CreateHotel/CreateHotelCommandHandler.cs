using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Hotels.CreateHotel;

public class CreateHotelCommandHandler(IRepository<Hotel> repository, IDateTime dateTime)
    : IRequestHandler<CreateHotelCommand, Result<HotelDto>>
{
    public async Task<Result<HotelDto>> Handle(
        CreateHotelCommand request,
        CancellationToken cancellationToken
    )
    {
        var hotel = new Hotel
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IcaoCode = request.IcaoCode,
            CreatedAt = dateTime.CetNow,
            CreatedBy = "Admin",
        };

        await repository.AddAsync(hotel, cancellationToken);

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
