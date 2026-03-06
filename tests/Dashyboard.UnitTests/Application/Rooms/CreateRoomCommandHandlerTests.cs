using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Commands.CreateRoom;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Rooms;

public class CreateRoomCommandHandlerTests
{
    private Mock<IRepository<Room>> _repositoryMock;
    private Mock<IDateTime> _dateTimeMock;

    private CreateRoomCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Room>>();
        _dateTimeMock = new Mock<IDateTime>();
        _dateTimeMock
            .Setup(x => x.CetNow)
            .Returns(new DateTime(2026, 3, 2, 12, 0, 0, DateTimeKind.Unspecified));

        _handler = new CreateRoomCommandHandler(_repositoryMock.Object, _dateTimeMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCreateRoom_AndReturnCreatedDto()
    {
        // Arrange
        Room? addedRoom = null;
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
            .Callback<Room, CancellationToken>((room, _) => addedRoom = room)
            .ReturnsAsync((Room r, CancellationToken _) => r);

        var command = new CreateRoomCommand(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            "999"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(
            result.Data.HotelId,
            Is.EqualTo(Guid.Parse("10000000-0000-0000-0000-000000000001"))
        );
        Assert.That(result.Data.RoomNumber, Is.EqualTo("999"));
    }
}
