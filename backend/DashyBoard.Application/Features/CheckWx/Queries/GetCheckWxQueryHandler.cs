using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.CheckWX;
using MediatR;

namespace DashyBoard.Application.Features.CheckWx.Queries;

public class GetCheckWxQueryHandler : IRequestHandler<GetCheckWxQuery, IEnumerable<CheckWxDto>>
{
    private readonly ICheckWxApiService _checkWxApiService;

    public GetCheckWxQueryHandler(ICheckWxApiService checkWxApiService)
    {
        _checkWxApiService = checkWxApiService;
    }

    public async Task<IEnumerable<CheckWxDto>> Handle(GetCheckWxQuery request, CancellationToken cancellationToken)
    {
        return await _checkWxApiService.GetCurrentWeatherAsync(
            request.Icao,
            cancellationToken);
    }
}