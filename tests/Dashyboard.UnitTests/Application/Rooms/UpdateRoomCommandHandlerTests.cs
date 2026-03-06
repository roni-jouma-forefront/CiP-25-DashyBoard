using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.UpdateRoom;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Rooms;

public class UpdateRoomCommandHandlerTests
{
    private Mock<IRepository<Room>> _repositoryMock;
    private Mock<IDateTime> _dateTimeMock;
    private UpdateRoomCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Room>>();
        _dateTimeMock = new Mock<IDateTime>();
        _dateTimeMock.Setup(x => x.CetNow).Returns(new DateTime(2026, 3, 5, 12, 0, 0));
        _handler = new UpdateRoomCommandHandler(_repositoryMock.Object, _dateTimeMock.Object);
    }

    [Test]
    public async Task Handle_ShouldUpdateRoom_AndReturnUpdatedDto()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var existingRoom = new Room
        {
            Id = roomId,
            HotelId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            RoomNumber = "101",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingRoom);

        var command = new UpdateRoomCommand(
            roomId,
            Guid.Parse("10000000-0000-0000-0000-000000000002"),
            "101A"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Id, Is.EqualTo(roomId));
        Assert.That(
            result.Data.HotelId,
            Is.EqualTo(Guid.Parse("10000000-0000-0000-0000-000000000002"))
        );
        Assert.That(result.Data.RoomNumber, Is.EqualTo("101A"));
    }

    [Test]
    public async Task Handle_ShouldReturnNull_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Room?)null);

        var command = new UpdateRoomCommand(
            roomId,
            Guid.Parse("10000000-0000-0000-0000-000000000002"),
            "101A"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Data, Is.Null);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task Handle_ShouldCallUpdateOnRepository_WhenRoomExists()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var existingRoom = new Room
        {
            Id = roomId,
            HotelId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            RoomNumber = "101",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingRoom);

        var command = new UpdateRoomCommand(
            roomId,
            Guid.Parse("10000000-0000-0000-0000-000000000002"),
            "101A"
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.UpdateAsync(It.Is<Room>(g => g.Id == roomId), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Test]
    public async Task Handle_ShouldNotCallUpdate_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Room?)null);

        var command = new UpdateRoomCommand(
            roomId,
            Guid.Parse("10000000-0000-0000-0000-000000000002"),
            "101A"
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}
