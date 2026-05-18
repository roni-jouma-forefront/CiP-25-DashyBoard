using DashyBoard.Application.DTOs;

namespace DashyBoard.Application.Common.Interfaces;

public interface IBookingCsvParser
{
    Task<IEnumerable<BookingCsvRowDto>> ParseAsync(
        Stream csvStream,
        CancellationToken cancellationToken = default
    );
}
