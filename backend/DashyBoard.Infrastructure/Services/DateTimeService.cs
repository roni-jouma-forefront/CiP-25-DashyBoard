using DashyBoard.Application.Common.Interfaces;

namespace DashyBoard.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    private static readonly string[] CetTimeZoneIds =
    [
        "Europe/Stockholm",
        "W. Europe Standard Time",
    ];

    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime CetNow => GetCurrentCetTime();

    private static DateTime GetCurrentCetTime()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var timeZoneId in CetTimeZoneIds)
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone);
            }
            catch (TimeZoneNotFoundException) { }
            catch (InvalidTimeZoneException) { }
        }

        return utcNow;
    }
}
