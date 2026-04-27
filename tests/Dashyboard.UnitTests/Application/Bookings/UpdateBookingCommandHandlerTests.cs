using System.Linq.Expressions;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.Bookings.UpdateBooking;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Bookings;

public class UpdateBookingCommandHandlerTests
{
    private Mock<IRepository<Booking>> _repositoryMock;
    private Mock<IDateTime> _dateTimeMock;
    private UpdateBookingCommandHandler _handler;

    private static readonly DateTime CheckIn = new DateTime(2026, 5, 1, 12, 0, 0);
    private static readonly DateTime CheckOut = new DateTime(2026, 5, 5, 12, 0, 0);

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Booking>>();
        _dateTimeMock = new Mock<IDateTime>();
        _dateTimeMock.Setup(x => x.CetNow).Returns(new DateTime(2026, 4, 8, 12, 0, 0));

        _repositoryMock
            .Setup(r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Booking, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new List<Booking>());

        _handler = new UpdateBookingCommandHandler(_repositoryMock.Object, _dateTimeMock.Object);
    }

    [Test]
    public async Task Handle_ShouldUpdateBooking_AndReturnUpdatedDto()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var existing = new Booking
        {
            Id = bookingId,
            NumberOfGuests = 1,
            CheckIn = CheckIn,
            CheckOut = CheckOut,
            BookingStatus = Booking.Status.Confirmed,
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var command = new UpdateBookingCommand(
            Id: bookingId,
            RoomId: null,
            GuestId: null,
            FlightNumber: "OS123",
            NumberOfGuests: 3,
            CheckIn: CheckIn,
            CheckOut: CheckOut,
            BookingStatus: Booking.Status.Active
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Id, Is.EqualTo(bookingId));
        Assert.That(result.Data.NumberOfGuests, Is.EqualTo(3));
        Assert.That(result.Data.BookingStatus, Is.EqualTo(Booking.Status.Active));
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking?)null);

        var command = new UpdateBookingCommand(
            Id: bookingId,
            RoomId: null,
            GuestId: null,
            FlightNumber: null,
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
    public async Task Handle_ShouldReturnFailure_WhenBookingIsCancelled()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var existing = new Booking
        {
            Id = bookingId,
            NumberOfGuests = 1,
            CheckIn = CheckIn,
            CheckOut = CheckOut,
            BookingStatus = Booking.Status.Cancelled,
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var command = new UpdateBookingCommand(
            Id: bookingId,
            RoomId: null,
            GuestId: null,
            FlightNumber: "OS123",
            NumberOfGuests: 2,
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
    public async Task Handle_ShouldReturnFailure_WhenBookingIsCompleted()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var existing = new Booking
        {
            Id = bookingId,
            NumberOfGuests = 1,
            CheckIn = CheckIn,
            CheckOut = CheckOut,
            BookingStatus = Booking.Status.Completed,
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var command = new UpdateBookingCommand(
            Id: bookingId,
            RoomId: null,
            GuestId: null,
            FlightNumber: "OS123",
            NumberOfGuests: 2,
            CheckIn: CheckIn,
            CheckOut: CheckOut,
            BookingStatus: Booking.Status.Active
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task Handle_ShouldCallUpdateOnRepository_WhenBookingExists()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var existing = new Booking
        {
            Id = bookingId,
            NumberOfGuests = 1,
            CheckIn = CheckIn,
            CheckOut = CheckOut,
            BookingStatus = Booking.Status.Confirmed,
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var command = new UpdateBookingCommand(
            Id: bookingId,
            RoomId: null,
            GuestId: null,
            FlightNumber: "OS123",
            NumberOfGuests: 2,
            CheckIn: CheckIn,
            CheckOut: CheckOut,
            BookingStatus: Booking.Status.Confirmed
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r =>
                r.UpdateAsync(
                    It.Is<Booking>(b => b.Id == bookingId),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
