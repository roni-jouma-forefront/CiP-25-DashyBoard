using System.Linq.Expressions;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Queries.Bookings.GetAllBookings;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Bookings;

public class GetAllBookingsQueryHandlerTests
{
    private Mock<IRepository<Booking>> _repositoryMock;
    private GetAllBookingsQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Booking>>();
        _handler = new GetAllBookingsQueryHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnAllBookings_WhenBookingsExist()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new Booking
            {
                Id = Guid.NewGuid(),
                NumberOfGuests = 1,
                CheckIn = new DateTime(2026, 5, 1),
                CheckOut = new DateTime(2026, 5, 3),
                BookingStatus = Booking.Status.Confirmed,
            },
            new Booking
            {
                Id = Guid.NewGuid(),
                NumberOfGuests = 2,
                CheckIn = new DateTime(2026, 6, 1),
                CheckOut = new DateTime(2026, 6, 5),
                BookingStatus = Booking.Status.Active,
            },
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(bookings);

        var query = new GetAllBookingsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task Handle_ShouldReturnEmptyList_WhenNoBookingsExist()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Booking>());

        var query = new GetAllBookingsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task Handle_ShouldMapBookingFieldsCorrectly()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var bookings = new List<Booking>
        {
            new Booking
            {
                Id = bookingId,
                RoomId = roomId,
                GuestId = guestId,
                NumberOfGuests = 3,
                CheckIn = new DateTime(2026, 5, 1),
                CheckOut = new DateTime(2026, 5, 7),
                BookingStatus = Booking.Status.Confirmed,
            },
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(bookings);

        var query = new GetAllBookingsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var dto = result.First();
        Assert.That(dto.Id, Is.EqualTo(bookingId));
        Assert.That(dto.RoomId, Is.EqualTo(roomId));
        Assert.That(dto.GuestId, Is.EqualTo(guestId));
        Assert.That(dto.NumberOfGuests, Is.EqualTo(3));
        Assert.That(dto.BookingStatus, Is.EqualTo(Booking.Status.Confirmed));
    }

    [Test]
    public async Task Handle_ShouldUseFilter_WhenGuestIdProvided()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var filtered = new List<Booking>
        {
            new Booking
            {
                Id = Guid.NewGuid(),
                GuestId = guestId,
                NumberOfGuests = 1,
                CheckIn = new DateTime(2026, 5, 1),
                CheckOut = new DateTime(2026, 5, 3),
                BookingStatus = Booking.Status.Confirmed,
            },
        };

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(filtered);

        var query = new GetAllBookingsQuery(GuestId: guestId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First().GuestId, Is.EqualTo(guestId));
        _repositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        _repositoryMock.Verify(
            r => r.FindAsync(It.IsAny<Expression<Func<Booking, bool>>>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
