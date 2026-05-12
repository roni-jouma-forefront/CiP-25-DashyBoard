using System.Globalization;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;

namespace DashyBoard.Infrastructure.Services;

public class BookingCsvParser : IBookingCsvParser
{
    private static readonly string[] RequiredHeaders =
    [
        "GuestFirstName",
        "GuestLastName",
        "RoomNumber",
        "NumberOfGuests",
        "CheckIn",
        "CheckOut",
        "BookingStatus",
        "FlightNumber",
        "FlightType",
    ];

    public async Task<IEnumerable<BookingCsvRowDto>> ParseAsync(
        Stream csvStream,
        CancellationToken cancellationToken = default
    )
    {
        var bookings = new List<BookingCsvRowDto>();
        using var reader = new StreamReader(csvStream);

        var headerLine = await reader.ReadLineAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(headerLine))
            throw new InvalidOperationException("CSV file is empty or missing header row.");

        var headers = headerLine.Split(',').Select(h => h.Trim()).ToArray();
        var missing = RequiredHeaders.Except(headers, StringComparer.OrdinalIgnoreCase).ToList();
        if (missing.Count > 0)
            throw new InvalidOperationException(
                "Missing required CSV headers: " + string.Join(", ", missing)
            );

        var lineNumber = 1;
        while (!reader.EndOfStream)
        {
            lineNumber++;
            var line = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var values = ParseCsvLine(line);
            if (values.Length != headers.Length)
                throw new FormatException(
                    "Line "
                        + lineNumber
                        + " has "
                        + values.Length
                        + " columns but expected "
                        + headers.Length
                        + "."
                );

            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < headers.Length; i++)
                dict[headers[i]] = values[i];

            bookings.Add(
                new BookingCsvRowDto
                {
                    GuestFirstName = dict["GuestFirstName"],
                    GuestLastName = dict["GuestLastName"],
                    RoomNumber = dict["RoomNumber"],
                    NumberOfGuests = int.Parse(
                        dict["NumberOfGuests"],
                        CultureInfo.InvariantCulture
                    ),
                    CheckIn = DateTime.Parse(
                        dict["CheckIn"],
                        CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.AssumeUniversal
                            | System.Globalization.DateTimeStyles.AdjustToUniversal
                    ),
                    CheckOut = DateTime.Parse(
                        dict["CheckOut"],
                        CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.AssumeUniversal
                            | System.Globalization.DateTimeStyles.AdjustToUniversal
                    ),
                    BookingStatus = dict["BookingStatus"],
                    FlightNumber = dict["FlightNumber"],
                    FlightType = dict["FlightType"],
                    BookingId = dict.TryGetValue("BookingId", out var bid)
                        && Guid.TryParse(bid, out var parsedId)
                        ? parsedId
                        : null,
                }
            );
        }

        return bookings;
    }

    private static string[] ParseCsvLine(string line)
    {
        var values = new List<string>();
        var current = "";
        var inQuotes = false;

        foreach (var c in line)
        {
            if (c == '"')
                inQuotes = !inQuotes;
            else if (c == ',' && !inQuotes)
            {
                values.Add(current.Trim());
                current = "";
            }
            else
                current += c;
        }
        values.Add(current.Trim());
        return values.ToArray();
    }
}
