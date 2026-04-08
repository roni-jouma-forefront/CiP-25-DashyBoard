using System.Linq.Expressions;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.Bookings.CreateBooking;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Bookings;

public class CreateBookingCommandHandlerTests
{
    private Mock<IRepository<Booking>> _repositoryMock;
    private Mock<IDateTime> _dateTimeMock;
    private CreateBookingCommandHandler _handler;

    private static readonly DateTime CheckIn = new DateTime(2026, 5, 1, 12, 0, 0);
    private static readonly DateTime CheckOut = new DateTime(2026, 5, 5, 12, 0, 0);

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Booking>>();
        _dateTimeMock = new Mock<IDateTime>();
        _dateTimeMock
            .Setup(x => x.CetNow)
            .Returns(new DateTime(2026, 4, 8, 12, 0, 0, DateTimeKind.Unspecified));

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Booking>());

        _handler = new CreateBookingCommandHandler(_repositoryMock.Object, _dateTimeMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WithNewBookingId()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking b, CancellationToken _) => b);

        var command = new CreateBookingCommand(
            RoomId: Guid.NewGuid(),
            GuestId: Guid.NewGuid(),
            FlightId: Guid.NewGuid(),
            NumberOfGuests: 2,
            CheckIn: CheckIn,
            CheckOut: CheckOut,
            BookingStatus: Booking.Status.Confirmed
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Data.NumberOfGuests, Is.EqualTo(2));
        Assert.That(result.Data.BookingStatus, Is.EqualTo(Booking.Status.Confirmed));
    }

    [Test]
    public async Task Handle_ShouldCallRepository_WithCorrectBookingData()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking b, CancellationToken _) => b);

        var command = new CreateBookingCommand(
            RoomId: roomId,
            GuestId: guestId,
            FlightId: null,
            NumberOfGuests: 1,
            CheckIn: CheckIn,
            CheckOut: CheckOut,
            BookingStatus: Booking.Status.Confirmed
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r =>
                r.AddAsync(
                    It.Is<Booking>(b =>
                        b.RoomId == roomId
                        && b.GuestId == guestId
                        && b.NumberOfGuests == 1
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenRoomHasOverlappingBooking()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var existing = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomId,
            CheckIn = CheckIn,
            CheckOut = CheckOut,
            BookingStatus = Booking.Status.Confirmed,
        };

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Booking> { existing });

        var command = new CreateBookingCommand(
            RoomId: roomId,
            GuestId: Guid.NewGuid(),
            FlightId: null,
            NumberOfGuests: 1,
            CheckIn: CheckIn,
            CheckOut: CheckOut,
            BookingStatus: Booking.Status.Confirmed
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenGuestHasOverlappingBooking()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var existing = new Booking
        {
            Id = Guid.NewGuid(),
            GuestId = guestId,
            CheckIn = CheckIn,
            CheckOut = CheckOut,
            BookingStatus = Booking.Status.Active,
        };

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Booking> { existing });

        var command = new CreateBookingCommand(
            RoomId: null,
            GuestId: guestId,
            FlightId: null,
            NumberOfGuests: 1,
            CheckIn: CheckIn,
            CheckOut: CheckOut,
            BookingStatus: Booking.Status.Confirmed
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }
}
