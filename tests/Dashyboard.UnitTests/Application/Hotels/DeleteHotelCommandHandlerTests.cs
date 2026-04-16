using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Application.Features.Commands.Hotels.DeleteHotel;
using DashyBoard.Domain.Entities;
using Moq;

namespace Dashyboard.UnitTests.Application.Hotels;

public class DeleteHotelCommandHandlerTests
{
    private Mock<IRepository<Hotel>> _repositoryMock;
    private DeleteHotelCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IRepository<Hotel>>();
        _handler = new DeleteHotelCommandHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnTrue_WhenHotelExistsAndIsDeleted()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel
        {
            Id = hotelId,
            Name = "Midlanda Airport Hotel",
            IcaoCode = "ESNN",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hotel);

        var command = new DeleteHotelCommand(hotelId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Data, Is.EqualTo(hotelId));
    }

    [Test]
    public async Task Handle_ShouldReturnFalse_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Hotel?)null);

        var command = new DeleteHotelCommand(hotelId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task Handle_ShouldCallDeleteOnRepository_WhenHotelExists()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel
        {
            Id = hotelId,
            Name = "Midlanda Airport Hotel",
            IcaoCode = "ESNN",
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hotel);

        var command = new DeleteHotelCommand(hotelId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(It.Is<Hotel>(g => g.Id == hotelId), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Test]
    public async Task Handle_ShouldNotCallDelete_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(hotelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Hotel?)null);

        var command = new DeleteHotelCommand(hotelId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}
