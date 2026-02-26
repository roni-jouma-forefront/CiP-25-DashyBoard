using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.DeleteGuest;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Guests;

public class DeleteGuestCommandHandlerTests
{
    private Mock<IRepository<Guest>> _repositoryMock;
    private DeleteGuestCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Guest>>();
        _handler = new DeleteGuestCommandHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnTrue_WhenGuestExistsAndIsDeleted()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = new Guest { Id = guestId, FirstName = "Alice", LastName = "Smith" };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(guest);

        var command = new DeleteGuestCommand(guestId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.EqualTo(guestId));
    }

    [Test]
    public async Task Handle_ShouldReturnFalse_WhenGuestDoesNotExist()
    {
        // Arrange
        var guestId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest?)null);

        var command = new DeleteGuestCommand(guestId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task Handle_ShouldCallDeleteOnRepository_WhenGuestExists()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = new Guest { Id = guestId, FirstName = "Alice", LastName = "Smith" };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(guest);

        var command = new DeleteGuestCommand(guestId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(It.Is<Guest>(g => g.Id == guestId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_ShouldNotCallDelete_WhenGuestDoesNotExist()
    {
        // Arrange
        var guestId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest?)null);

        var command = new DeleteGuestCommand(guestId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(It.IsAny<Guest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
