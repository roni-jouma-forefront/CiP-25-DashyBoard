using System.Linq.Expressions;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Queries.GetRoom;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Rooms;

public class GetRoomQueryHandlerTests
{
    private Mock<IRepository<Room>> _repositoryMock;
    private GetRoomQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Room>>();
        _handler = new GetRoomQueryHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnRoom_WhenRoomExists()
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
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Room> { room });

        var query = new GetRoomQuery(Guid.Parse("10000000-0000-0000-0000-000000000002"), roomId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(roomId));
        Assert.That(result.HotelId, Is.EqualTo(Guid.Parse("10000000-0000-0000-0000-000000000002")));
        Assert.That(result.RoomNumber, Is.EqualTo("Presidential Suite"));
    }

    [Test]
    public async Task Handle_ShouldReturnNull_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Room>());

        var query = new GetRoomQuery(Guid.Parse("10000000-0000-0000-0000-000000000002"), roomId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Handle_ShouldCallRepositoryWithCorrectFilter()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Room>());

        var query = new GetRoomQuery(Guid.Parse("10000000-0000-0000-0000-000000000002"), roomId);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Room, bool>>>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
