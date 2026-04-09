using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Queries.Bookings.GetBooking;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Bookings;

public class GetBookingQueryHandlerTests
{
    private Mock<IRepository<Booking>> _repositoryMock;
    private GetBookingQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Booking>>();
        _handler = new GetBookingQueryHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnBooking_WhenBookingExists()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var booking = new Booking
        {
            Id = bookingId,
            RoomId = roomId,
            GuestId = guestId,
            NumberOfGuests = 2,
            CheckIn = new DateTime(2026, 5, 1),
            CheckOut = new DateTime(2026, 5, 5),
            BookingStatus = Booking.Status.Confirmed,
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(booking);

        var query = new GetBookingQuery(bookingId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(bookingId));
        Assert.That(result.RoomId, Is.EqualTo(roomId));
        Assert.That(result.GuestId, Is.EqualTo(guestId));
        Assert.That(result.NumberOfGuests, Is.EqualTo(2));
        Assert.That(result.BookingStatus, Is.EqualTo(Booking.Status.Confirmed));
    }

    [Test]
    public async Task Handle_ShouldReturnNull_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking?)null);

        var query = new GetBookingQuery(bookingId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
    }
}
