using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.Hotels.CreateHotel;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Hotels;

public class CreateHotelCommandHandlerTests
{
    private Mock<IRepository<Hotel>> _repositoryMock;
    private Mock<IDateTime> _dateTimeMock;

    private CreateHotelCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Hotel>>();
        _dateTimeMock = new Mock<IDateTime>();
        _dateTimeMock
            .Setup(x => x.CetNow)
            .Returns(new DateTime(2026, 3, 2, 12, 0, 0, DateTimeKind.Unspecified));

        _handler = new CreateHotelCommandHandler(_repositoryMock.Object, _dateTimeMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCreateHotel_AndReturnCreatedDto()
    {
        // Arrange
        Hotel? addedHotel = null;
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()))
            .Callback<Hotel, CancellationToken>((hotel, _) => addedHotel = hotel)
            .ReturnsAsync((Hotel h, CancellationToken _) => h);

        var command = new CreateHotelCommand("Midlanda Airport Hotel", "ESNN");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Data.Name, Is.EqualTo("Midlanda Airport Hotel"));
        Assert.That(result.Data.IcaoCode, Is.EqualTo("ESNN"));
    }
}
