using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.UpdateGuest;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Guests;

public class UpdateGuestCommandHandlerTests
{
    private Mock<IRepository<Guest>> _repositoryMock;
    private Mock<IDateTime> _dateTimeMock;
    private UpdateGuestCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Guest>>();
        _dateTimeMock = new Mock<IDateTime>();
        _dateTimeMock.Setup(x => x.CetNow).Returns(new DateTime(2026, 3, 5, 12, 0, 0));
        _handler = new UpdateGuestCommandHandler(_repositoryMock.Object, _dateTimeMock.Object);
    }

    [Test]
    public async Task Handle_ShouldUpdateGuest_AndReturnUpdatedDto()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var existingGuest = new Guest
        {
            Id = guestId,
            FirstName = "Old",
            LastName = "Name",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingGuest);

        var command = new UpdateGuestCommand(guestId, "Alice", "Smith");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Id, Is.EqualTo(guestId));
        Assert.That(result.Data.FirstName, Is.EqualTo("Alice"));
        Assert.That(result.Data.LastName, Is.EqualTo("Smith"));
    }

    [Test]
    public async Task Handle_ShouldReturnNull_WhenGuestDoesNotExist()
    {
        // Arrange
        var guestId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest?)null);

        var command = new UpdateGuestCommand(guestId, "Alice", "Smith");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Data, Is.Null);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task Handle_ShouldCallUpdateOnRepository_WhenGuestExists()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var existingGuest = new Guest
        {
            Id = guestId,
            FirstName = "Old",
            LastName = "Name",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingGuest);

        var command = new UpdateGuestCommand(guestId, "Alice", "Smith");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.UpdateAsync(It.Is<Guest>(g => g.Id == guestId), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Test]
    public async Task Handle_ShouldNotCallUpdate_WhenGuestDoesNotExist()
    {
        // Arrange
        var guestId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(guestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guest?)null);

        var command = new UpdateGuestCommand(guestId, "Alice", "Smith");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Guest>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}
