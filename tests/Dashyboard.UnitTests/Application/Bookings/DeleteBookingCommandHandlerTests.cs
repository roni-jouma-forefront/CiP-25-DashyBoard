using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.Bookings.DeleteBooking;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Bookings;

public class DeleteBookingCommandHandlerTests
{
    private Mock<IRepository<Booking>> _repositoryMock;
    private DeleteBookingCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Booking>>();
        _handler = new DeleteBookingCommandHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenBookingExistsAndIsDeleted()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = new Booking
        {
            Id = bookingId,
            NumberOfGuests = 1,
            CheckIn = new DateTime(2026, 5, 1),
            CheckOut = new DateTime(2026, 5, 5),
            BookingStatus = Booking.Status.Confirmed,
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(booking);

        var command = new DeleteBookingCommand(bookingId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.EqualTo(bookingId));
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking?)null);

        var command = new DeleteBookingCommand(bookingId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task Handle_ShouldCallDeleteOnRepository_WhenBookingExists()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = new Booking
        {
            Id = bookingId,
            NumberOfGuests = 1,
            CheckIn = new DateTime(2026, 5, 1),
            CheckOut = new DateTime(2026, 5, 5),
            BookingStatus = Booking.Status.Confirmed,
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(booking);

        var command = new DeleteBookingCommand(bookingId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r =>
                r.DeleteAsync(
                    It.Is<Booking>(b => b.Id == bookingId),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
