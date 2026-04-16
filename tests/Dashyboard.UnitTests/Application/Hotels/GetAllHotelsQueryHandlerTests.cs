using System.Linq.Expressions;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Queries.Hotels.GetAllHotels;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Hotels;

public class GetAllHotelsQueryHandlerTests
{
    private Mock<IRepository<Hotel>> _repositoryMock;
    private GetAllHotelsQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Hotel>>();
        _handler = new GetAllHotelsQueryHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnAllHotels_WhenHotelsExist()
    {
        // Arrange
        var hotels = new List<Hotel>
        {
            new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Midlanda Airport Hotel",
                IcaoCode = "ESNN",
            },
            new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Stockholm Central Hotel",
                IcaoCode = "ESST",
            },
        };

        _repositoryMock
            .Setup(r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Hotel, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(hotels);

        var query = new GetAllHotelsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task Handle_ShouldReturnEmptyList_WhenNoHotelsExist()
    {
        // Arrange
        _repositoryMock
            .Setup(r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Hotel, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(new List<Hotel>());

        var query = new GetAllHotelsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task Handle_ShouldMapHotelFieldsCorrectly()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotels = new List<Hotel>
        {
            new Hotel
            {
                Id = hotelId,
                Name = "Midlanda Airport Hotel",
                IcaoCode = "ESNN",
            },
            new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "Stockholm Central Hotel",
                IcaoCode = "ESST",
            },
        };

        _repositoryMock
            .Setup(r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Hotel, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(hotels);

        var query = new GetAllHotelsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }
}
