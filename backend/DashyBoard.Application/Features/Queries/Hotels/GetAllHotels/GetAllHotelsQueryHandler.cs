using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Hotels.GetAllHotels;

public class GetAllHotelsQueryHandler(IRepository<Hotel> repository)
    : IRequestHandler<GetAllHotelsQuery, List<HotelDto>>
{
    public async Task<List<HotelDto>> Handle(
        GetAllHotelsQuery request,
        CancellationToken cancellationToken
    )
    {
        var hotels = await repository.FindAsync(
            r => true,
            cancellationToken
        );

        return hotels
            .Select(r => new HotelDto
            {
                Id = r.Id,
                Name = r.Name,
                IcaoCode = r.IcaoCode,
            })
            .ToList();
    }
}
