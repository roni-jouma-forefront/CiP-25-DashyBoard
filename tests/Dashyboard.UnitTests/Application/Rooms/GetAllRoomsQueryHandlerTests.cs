using System.Linq.Expressions;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Queries.GetAllRooms;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Rooms;

public class GetAllRoomsQueryHandlerTests
{
    private Mock<IRepository<Room>> _repositoryMock;
    private GetAllRoomsQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Room>>();
        _handler = new GetAllRoomsQueryHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnAllRooms_WhenRoomsExist()
    {
        // Arrange
        var rooms = new List<Room>
        {
            new Room
            {
                Id = Guid.NewGuid(),
                HotelId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                RoomNumber = "Presidential Suite",
            },
            new Room
            {
                Id = Guid.NewGuid(),
                HotelId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                RoomNumber = "1426",
            },
        };

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(rooms);

        var query = new GetAllRoomsQuery(Guid.Parse("10000000-0000-0000-0000-000000000002"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task Handle_ShouldReturnEmptyList_WhenNoRoomsExist()
    {
        // Arrange
        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new List<Room>());

        var query = new GetAllRoomsQuery(Guid.Parse("10000000-0000-0000-0000-000000000002"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task Handle_ShouldMapRoomFieldsCorrectly()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var rooms = new List<Room>
        {
            new Room
            {
                Id = roomId,
                HotelId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                RoomNumber = "Presidential Suite",
            },
        };

        _repositoryMock
            .Setup(r =>
                r.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(rooms);

        var query = new GetAllRoomsQuery(Guid.Parse("10000000-0000-0000-0000-000000000002"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        var dto = result.First();

        // Assert
        Assert.That(dto.Id, Is.EqualTo(roomId));
        Assert.That(dto.RoomNumber, Is.EqualTo("Presidential Suite"));
        Assert.That(dto.HotelId, Is.EqualTo(Guid.Parse("10000000-0000-0000-0000-000000000002")));
    }
}
