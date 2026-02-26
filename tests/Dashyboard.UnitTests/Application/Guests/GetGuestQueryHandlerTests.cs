using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Queries.GetGuest;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Guests;

public class GetGuestQueryHandlerTests
{
    private Mock<IRepository<Guest>> _repositoryMock;
    private GetGuestQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Guest>>();
        _handler = new GetGuestQueryHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnGuest_WhenGuestExists()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = new Guest { Id = guestId, FirstName = "Alice", LastName = "Smith" };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(guest);

        var query = new GetGuestQuery(guestId, "Alice", "Smith");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(guestId));
        Assert.That(result.FirstName, Is.EqualTo("Alice"));
        Assert.That(result.LastName, Is.EqualTo("Smith"));
    }

    [Test]
    public async Task Handle_ShouldReturnNull_WhenGuestDoesNotExist()
    {
        // Arrange
        var guestId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest?)null);

        var query = new GetGuestQuery(guestId, "Alice", "Smith");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var guestId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest?)null);

        var query = new GetGuestQuery(guestId, "Alice", "Smith");

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
