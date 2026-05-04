using System.Linq.Expressions;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Common.Interfaces.External;
using DashyBoard.Application.DTOs;
using DashyBoard.Application.DTOs.Swedavia;
using DashyBoard.Application.Features.Commands.ImportBookingsFromCsv;
using DashyBoard.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dashyboard.UnitTests.Application.Bookings;

public class ImportBookingsFromCsvHandlerTests
{
    private Mock<IBookingCsvParser> _parserMock;
    private Mock<IRepository<Booking>> _bookingRepoMock;
    private Mock<IRepository<Guest>> _guestRepoMock;
    private Mock<IRepository<Room>> _roomRepoMock;
    private Mock<ISwedaviaFlightApiService> _flightApiMock;
    private Mock<IDateTime> _dateTimeMock;
    private Mock<ILogger<ImportBookingsFromCsvHandler>> _loggerMock;
    private ImportBookingsFromCsvHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _parserMock = new Mock<IBookingCsvParser>();
        _bookingRepoMock = new Mock<IRepository<Booking>>();
        _guestRepoMock = new Mock<IRepository<Guest>>();
        _roomRepoMock = new Mock<IRepository<Room>>();
        _flightApiMock = new Mock<ISwedaviaFlightApiService>();
        _dateTimeMock = new Mock<IDateTime>();
        _loggerMock = new Mock<ILogger<ImportBookingsFromCsvHandler>>();

        _dateTimeMock.Setup(x => x.CetNow).Returns(new DateTime(2026, 4, 21, 12, 0, 0));

        _handler = new ImportBookingsFromCsvHandler(
            _parserMock.Object,
            _bookingRepoMock.Object,
            _guestRepoMock.Object,
            _roomRepoMock.Object,
            _dateTimeMock.Object,
            _loggerMock.Object
        );
    }

    private static BookingCsvRowDto CreateRow(
        string firstName = "John",
        string lastName = "Doe",
        string roomNumber = "101",
        string flightNumber = "SK1234",
        string flightType = "Arrival"
    )
    {
        return new BookingCsvRowDto
        {
            GuestFirstName = firstName,
            GuestLastName = lastName,
            RoomNumber = roomNumber,
            NumberOfGuests = 2,
            CheckIn = new DateTime(2026, 5, 1),
            CheckOut = new DateTime(2026, 5, 5),
            BookingStatus = "Confirmed",
            FlightNumber = flightNumber,
            FlightType = flightType,
        };
    }

    [Test]
    public async Task Handle_ValidRow_CreatesBookingWithGuestRoomAndFlight()
    {
        var row = CreateRow();
        _parserMock
            .Setup(p => p.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { row });

        // Guest not found -> will be created
        _guestRepoMock
            .Setup(r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Guest, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(Enumerable.Empty<Guest>());
        _guestRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Guest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest g, CancellationToken _) => g);

        // Room found
        var room = new Room
        {
            Id = Guid.NewGuid(),
            HotelId = Guid.NewGuid(),
            RoomNumber = "101",
        };
        _roomRepoMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new[] { room });

        // Flight API returns data
        var flightDto = new FlightInfoDto
        {
            FlightId = "SK1234",
            LocationAndStatus = new LocationAndStatusDto
            {
                Gate = "A12",
                FlightLegStatusEnglish = "Landed",
            },
            ArrivalTime = new FlightTimeDto { ScheduledUtc = new DateTime(2026, 5, 1, 10, 0, 0) },
        };
        _flightApiMock
            .Setup(a =>
                a.GetArrivalsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new[] { flightDto });

        // Booking add
        _bookingRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking b, CancellationToken _) => b);

        using var stream = new MemoryStream();
        var result = await _handler.Handle(
            new ImportBookingsFromCsvCommand(stream),
            CancellationToken.None
        );

        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data!.TotalRows, Is.EqualTo(1));
        Assert.That(result.Data.SuccessfulImports, Is.EqualTo(1));
        Assert.That(result.Data.FailedImports, Is.EqualTo(0));
        Assert.That(result.Data.CreatedBookingIds, Has.Count.EqualTo(1));

        _guestRepoMock.Verify(
            r =>
                r.AddAsync(
                    It.Is<Guest>(g => g.FirstName == "John" && g.LastName == "Doe"),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        _bookingRepoMock.Verify(
            r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Test]
    public async Task Handle_ExistingGuest_DoesNotCreateDuplicate()
    {
        var row = CreateRow();
        _parserMock
            .Setup(p => p.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { row });

        var existingGuest = new Guest
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            IsPilot = false,
        };
        _guestRepoMock
            .Setup(r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Guest, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new[] { existingGuest });

        var room = new Room
        {
            Id = Guid.NewGuid(),
            HotelId = Guid.NewGuid(),
            RoomNumber = "101",
        };
        _roomRepoMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new[] { room });

        _flightApiMock
            .Setup(a =>
                a.GetArrivalsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(Enumerable.Empty<FlightInfoDto>());

        _bookingRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking b, CancellationToken _) => b);

        using var stream = new MemoryStream();
        var result = await _handler.Handle(
            new ImportBookingsFromCsvCommand(stream),
            CancellationToken.None
        );

        Assert.That(result.Succeeded, Is.True);
        _guestRepoMock.Verify(
            r => r.AddAsync(It.IsAny<Guest>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Test]
    public async Task Handle_RoomNotFound_RecordsError()
    {
        var row = CreateRow(roomNumber: "999");
        _parserMock
            .Setup(p => p.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { row });

        _guestRepoMock
            .Setup(r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Guest, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(Enumerable.Empty<Guest>());
        _guestRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Guest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest g, CancellationToken _) => g);

        _roomRepoMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(Enumerable.Empty<Room>());

        using var stream = new MemoryStream();
        var result = await _handler.Handle(
            new ImportBookingsFromCsvCommand(stream),
            CancellationToken.None
        );

        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data!.FailedImports, Is.EqualTo(1));
        Assert.That(result.Data.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Data.Errors[0], Does.Contain("Room 999 not found"));
    }

    [Test]
    public async Task Handle_ParseFails_ReturnsFailure()
    {
        _parserMock
            .Setup(p => p.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Bad CSV"));

        using var stream = new MemoryStream();
        var result = await _handler.Handle(
            new ImportBookingsFromCsvCommand(stream),
            CancellationToken.None
        );

        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Does.Contain("Failed to parse CSV: Bad CSV"));
    }
}
