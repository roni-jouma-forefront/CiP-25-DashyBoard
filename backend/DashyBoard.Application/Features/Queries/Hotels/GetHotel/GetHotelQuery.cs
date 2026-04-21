using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Hotels.GetHotel;

public record GetHotelQuery(Guid HotelId) : IRequest<HotelDto?>;
