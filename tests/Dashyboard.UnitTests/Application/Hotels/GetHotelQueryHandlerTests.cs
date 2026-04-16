using System.Linq.Expressions;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Queries.Hotels.GetHotel;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Hotels;

public class GetHotelQueryHandlerTests
{
    private Mock<IRepository<Hotel>> _repositoryMock;
    private GetHotelQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Hotel>>();
        _handler = new GetHotelQueryHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnHotel_WhenHotelExists()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel
        {
            Id = hotelId,
            Name = "Midlanda Airport Hotel",
            IcaoCode = "ESNN",
        };

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Hotel, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Hotel> { hotel });

        var query = new GetHotelQuery(hotelId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(hotelId));
        Assert.That(result.Name, Is.EqualTo("Midlanda Airport Hotel"));
        Assert.That(result.IcaoCode, Is.EqualTo("ESNN"));
    }

    [Test]
    public async Task Handle_ShouldReturnNull_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Hotel, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Hotel>());

        var query = new GetHotelQuery(hotelId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Handle_ShouldCallRepositoryWithCorrectFilter()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Hotel, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Hotel>());

        var query = new GetHotelQuery(hotelId);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Hotel, bool>>>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
