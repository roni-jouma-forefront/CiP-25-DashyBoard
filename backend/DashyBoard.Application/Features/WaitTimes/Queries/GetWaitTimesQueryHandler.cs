using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs.Swedavia;
using MediatR;

namespace DashyBoard.Application.Features.WaitTimes.Queries;

public class GetWaitTimesQueryHandler : IRequestHandler<GetWaitTimesQuery, IEnumerable<WaitTimeDto>>
{
    private readonly ISwedaviaWaitTimeApiService _waitTimeApiService;

    public GetWaitTimesQueryHandler(ISwedaviaWaitTimeApiService waitTimeApiService)
    {
        _waitTimeApiService = waitTimeApiService;
    }

    public async Task<IEnumerable<WaitTimeDto>> Handle(GetWaitTimesQuery request, CancellationToken cancellationToken)
    {
        return await _waitTimeApiService.GetWaitTimesAsync(
            request.Airport,
            request.Date,
            cancellationToken);
    }
}
