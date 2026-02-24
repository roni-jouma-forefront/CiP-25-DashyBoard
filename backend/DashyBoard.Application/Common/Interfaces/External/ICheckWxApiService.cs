using DashyBoard.Application.DTOs.CheckWX;

namespace DashyBoard.Application.Common.Interfaces.External;

public interface ICheckWxApiService
{
    Task<IEnumerable<CheckWxDto>> GetCurrentWeatherAsync(string icao, CancellationToken cancellationToken);
}