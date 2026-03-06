using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.DTOs;
using DashyBoard.Application.Features.Commands.CreateGuest;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Guests;

public class CreateGuestCommandHandlerTests
{
    private Mock<IRepository<Guest>> _repositoryMock;
    private Mock<IDateTime> _dateTimeMock;
    private CreateGuestCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Guest>>();
        _dateTimeMock = new Mock<IDateTime>();
        _dateTimeMock
            .Setup(x => x.CetNow)
            .Returns(new DateTime(2026, 3, 2, 12, 0, 0, DateTimeKind.Unspecified));

        _handler = new CreateGuestCommandHandler(_repositoryMock.Object, _dateTimeMock.Object);
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
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Data.FirstName, Is.EqualTo("Alice"));
        Assert.That(result.Data.LastName, Is.EqualTo("Smith"));
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
        _repositoryMock.Verify(
            r =>
                r.AddAsync(
                    It.Is<Guest>(g => g.FirstName == "Bob" && g.LastName == "Jones"),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
