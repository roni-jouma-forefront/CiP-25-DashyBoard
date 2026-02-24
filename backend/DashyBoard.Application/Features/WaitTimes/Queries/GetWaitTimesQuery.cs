using DashyBoard.Application.DTOs.Swedavia;
using MediatR;

namespace DashyBoard.Application.Features.WaitTimes.Queries;

public record GetWaitTimesQuery(
    string Airport,
    DateOnly Date
) : IRequest<IEnumerable<WaitTimeDto>>;
