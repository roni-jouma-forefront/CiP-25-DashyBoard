using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.DeleteRoom;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Rooms;

public class DeleteRoomCommandHandlerTests
{
    private Mock<IRepository<Room>> _repositoryMock;
    private DeleteRoomCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Room>>();
        _handler = new DeleteRoomCommandHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnTrue_WhenRoomExistsAndIsDeleted()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = new Room
        {
            Id = roomId,
            HotelId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            RoomNumber = "Presidential Suite",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);

        var command = new DeleteRoomCommand(
            roomId,
            Guid.Parse("10000000-0000-0000-0000-000000000002")
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.EqualTo(roomId));
    }

    [Test]
    public async Task Handle_ShouldReturnFalse_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Room?)null);

        var command = new DeleteRoomCommand(
            roomId,
            Guid.Parse("10000000-0000-0000-0000-000000000002")
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task Handle_ShouldCallDeleteOnRepository_WhenRoomExists()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = new Room
        {
            Id = roomId,
            HotelId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            RoomNumber = "Presidential Suite",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);

        var command = new DeleteRoomCommand(
            roomId,
            Guid.Parse("10000000-0000-0000-0000-000000000002")
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(It.Is<Room>(g => g.Id == roomId), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Test]
    public async Task Handle_ShouldNotCallDelete_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Room?)null);

        var command = new DeleteRoomCommand(
            roomId,
            Guid.Parse("10000000-0000-0000-0000-000000000002")
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}
