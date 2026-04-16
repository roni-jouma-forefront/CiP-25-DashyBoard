using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Hotels.GetHotel;

public class GetHotelQueryHandler(IRepository<Hotel> repository)
    : IRequestHandler<GetHotelQuery, HotelDto?>
{
    public async Task<HotelDto?> Handle(GetHotelQuery request, CancellationToken cancellationToken)
    {
        var hotel = (
            await repository.FindAsync(r => r.Id == request.HotelId, cancellationToken)
        ).FirstOrDefault();

        if (hotel == null)
        {
            return null;
        }

        return new HotelDto
        {
            Id = hotel.Id,
            Name = hotel.Name,
            IcaoCode = hotel.IcaoCode,
        };
    }
}
