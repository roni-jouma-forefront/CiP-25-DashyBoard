using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Models;
using DashyBoard.Domain.Entities;
using MediatR;

namespace DashyBoard.Application.Features.Commands.Hotels.DeleteHotel;

public class DeleteHotelCommandHandler(IRepository<Hotel> repository)
    : IRequestHandler<DeleteHotelCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        DeleteHotelCommand request,
        CancellationToken cancellationToken
    )
    {
        var hotel = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (hotel == null)
        {
            return Result<Guid>.Failure("Hotel not found.");
        }

        await repository.DeleteAsync(hotel, cancellationToken);

        return Result<Guid>.Success(hotel.Id);
    }
}
