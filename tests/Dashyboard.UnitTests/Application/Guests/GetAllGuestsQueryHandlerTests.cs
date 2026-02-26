using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Queries.GetAllGuests;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Guests;

public class GetAllGuestsQueryHandlerTests
{
    private Mock<IRepository<Guest>> _repositoryMock;
    private GetAllGuestsQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Guest>>();
        _handler = new GetAllGuestsQueryHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnAllGuests_WhenGuestsExist()
    {
        // Arrange
        var guests = new List<Guest>
        {
            new Guest { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Smith" },
            new Guest { Id = Guid.NewGuid(), FirstName = "Bob",   LastName = "Jones" },
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(guests);

        var query = new GetAllGuestsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task Handle_ShouldReturnEmptyList_WhenNoGuestsExist()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Guest>());

        var query = new GetAllGuestsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task Handle_ShouldMapGuestFieldsCorrectly()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guests = new List<Guest>
        {
            new Guest { Id = guestId, FirstName = "Alice", LastName = "Smith" }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(guests);

        var query = new GetAllGuestsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        var dto = result.First();

        // Assert
        Assert.That(dto.Id, Is.EqualTo(guestId));
        Assert.That(dto.FirstName, Is.EqualTo("Alice"));
        Assert.That(dto.LastName, Is.EqualTo("Smith"));
    }
}
