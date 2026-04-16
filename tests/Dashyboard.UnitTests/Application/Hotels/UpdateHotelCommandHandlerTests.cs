using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.Hotels.UpdateHotel;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Hotels.UpdateHotel;

public class UpdateHotelCommandHandlerTests
{
    private Mock<IRepository<Hotel>> _repositoryMock;
    private Mock<IDateTime> _dateTimeMock;
    private UpdateHotelCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Hotel>>();
        _dateTimeMock = new Mock<IDateTime>();
        _dateTimeMock.Setup(x => x.CetNow).Returns(new DateTime(2026, 4, 16, 12, 0, 0));
        _handler = new UpdateHotelCommandHandler(_repositoryMock.Object, _dateTimeMock.Object);
    }

    [Test]
    public async Task Handle_ShouldUpdateHotel_AndReturnUpdatedDto()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var existingHotel = new Hotel
        {
            Id = hotelId,
            Name = "Old Hotel Name",
            IcaoCode = "OLD1",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHotel);

        var command = new UpdateHotelCommand(hotelId, "New Hotel Name", "NEW1");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data!.Id, Is.EqualTo(hotelId));
        Assert.That(result.Data.Name, Is.EqualTo("New Hotel Name"));
        Assert.That(result.Data.IcaoCode, Is.EqualTo("NEW1"));
    }

    [Test]
    public async Task Handle_ShouldReturnNull_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Hotel?)null);

        var command = new UpdateHotelCommand(hotelId, "New Hotel Name", "NEW1");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Data, Is.Null);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task Handle_ShouldCallUpdateOnRepository_WhenHotelExists()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var existingHotel = new Hotel
        {
            Id = hotelId,
            Name = "Old Hotel Name",
            IcaoCode = "OLD1",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHotel);

        var command = new UpdateHotelCommand(hotelId, "New Hotel Name", "NEW1");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.UpdateAsync(It.Is<Hotel>(g => g.Id == hotelId), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Test]
    public async Task Handle_ShouldNotCallUpdate_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Hotel?)null);

        var command = new UpdateHotelCommand(hotelId, "New Hotel Name", "NEW1");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}
