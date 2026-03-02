using DashyBoard.Application.DTOs.CheckWX;
using MediatR;

namespace DashyBoard.Application.Features.CheckWx.Queries;

public record GetCheckWxQuery(string Icao) : IRequest<IEnumerable<CheckWxDto>>;
