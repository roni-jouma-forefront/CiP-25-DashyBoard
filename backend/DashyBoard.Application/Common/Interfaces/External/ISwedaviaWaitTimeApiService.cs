using DashyBoard.Application.DTOs.Swedavia;

namespace DashyBoard.Application.Common.Interfaces.External;

public interface ISwedaviaWaitTimeApiService
{
    Task<IEnumerable<WaitTimeDto>> GetWaitTimesAsync(
        string airport,
        DateOnly? date = null,
        CancellationToken cancellationToken = default
    );
}
