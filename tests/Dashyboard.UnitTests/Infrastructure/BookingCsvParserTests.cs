using System.Text;
using DashyBoard.Infrastructure.Services;

namespace Dashyboard.UnitTests.Infrastructure;

public class BookingCsvParserTests
{
    private BookingCsvParser _parser;

    [SetUp]
    public void SetUp()
    {
        _parser = new BookingCsvParser();
    }

    private static Stream ToStream(string content)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(content));
    }

    [Test]
    public async Task ParseAsync_ValidCsv_ReturnsAllRows()
    {
        var csv =
            "GuestFirstName,GuestLastName,RoomNumber,NumberOfGuests,CheckIn,CheckOut,BookingStatus,FlightNumber,FlightType\n"
            + "John,Doe,101,2,2026-05-01,2026-05-05,Confirmed,SK1234,Arrival\n"
            + "Jane,Smith,102,1,2026-05-02,2026-05-04,Confirmed,LH2045,Departure\n";

        using var stream = ToStream(csv);
        var result = (await _parser.ParseAsync(stream)).ToList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].GuestFirstName, Is.EqualTo("John"));
        Assert.That(result[0].GuestLastName, Is.EqualTo("Doe"));
        Assert.That(result[0].RoomNumber, Is.EqualTo("101"));
        Assert.That(result[0].NumberOfGuests, Is.EqualTo(2));
        Assert.That(result[0].FlightNumber, Is.EqualTo("SK1234"));
        Assert.That(result[0].FlightType, Is.EqualTo("Arrival"));
        Assert.That(result[1].GuestFirstName, Is.EqualTo("Jane"));
        Assert.That(result[1].FlightType, Is.EqualTo("Departure"));
    }

    [Test]
    public void ParseAsync_EmptyStream_ThrowsInvalidOperationException()
    {
        using var stream = ToStream("");
        Assert.ThrowsAsync<InvalidOperationException>(() => _parser.ParseAsync(stream));
    }

    [Test]
    public void ParseAsync_MissingHeaders_ThrowsInvalidOperationException()
    {
        var csv = "GuestFirstName,GuestLastName\nJohn,Doe\n";
        using var stream = ToStream(csv);
        Assert.ThrowsAsync<InvalidOperationException>(() => _parser.ParseAsync(stream));
    }

    [Test]
    public void ParseAsync_WrongColumnCount_ThrowsFormatException()
    {
        var csv =
            "GuestFirstName,GuestLastName,RoomNumber,NumberOfGuests,CheckIn,CheckOut,BookingStatus,FlightNumber,FlightType\n"
            + "John,Doe\n";
        using var stream = ToStream(csv);
        Assert.ThrowsAsync<FormatException>(() => _parser.ParseAsync(stream));
    }

    [Test]
    public async Task ParseAsync_SkipsBlankLines()
    {
        var csv =
            "GuestFirstName,GuestLastName,RoomNumber,NumberOfGuests,CheckIn,CheckOut,BookingStatus,FlightNumber,FlightType\n"
            + "\n"
            + "John,Doe,101,2,2026-05-01,2026-05-05,Confirmed,SK1234,Arrival\n"
            + "\n";

        using var stream = ToStream(csv);
        var result = (await _parser.ParseAsync(stream)).ToList();
        Assert.That(result, Has.Count.EqualTo(1));
    }
}
