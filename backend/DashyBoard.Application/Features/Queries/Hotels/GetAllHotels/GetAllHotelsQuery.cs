using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Queries.Hotels.GetAllHotels;

public record GetAllHotelsQuery() : IRequest<List<HotelDto>>;
