using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.CreateGuest;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Guests;

public class CreateGuestCommandHandlerTests
{
    private Mock<IRepository<Guest>> _repositoryMock;
    private CreateGuestCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Guest>>();
        _handler = new CreateGuestCommandHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WithNewGuestId()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Guest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest g, CancellationToken _) => g);

        var command = new CreateGuestCommand("Alice", "Smith");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data,      Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task Handle_ShouldCallRepository_WithCorrectGuestData()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Guest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest g, CancellationToken _) => g);

        var command = new CreateGuestCommand("Bob", "Jones");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(
            It.Is<Guest>(g => g.FirstName == "Bob" && g.LastName == "Jones"),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
