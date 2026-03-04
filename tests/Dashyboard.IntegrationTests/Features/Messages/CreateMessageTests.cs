using System.Net;
using System.Net.Http.Json;
using DashyBoard.Application.Common.Models;
using DashyBoard.Application.Features.Messages.Commands.CreateMessage;
using DashyBoard.Domain.Entities;
using DashyBoard.Infrastructure.Persistence;
using Dashyboard.IntegrationTests.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Dashyboard.IntegrationTests.Features.Messages;

public class CreateMessageTests
{
    private CustomWebApplicationFactory _factory = null!;
    private HttpClient _client = null!;
    private ApplicationDbContext _context = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
        var scope = _factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task CreateMessage_WithHotelId_ShouldReturnSuccess()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel
        {
            Id = hotelId,
            Name = "Test Hotel",
            CreatedAt = DateTime.UtcNow,
        };
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        var command = new CreateMessageCommand
        {
            HotelId = hotelId,
            Content = "Frukosten stänger 10:00",
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Messages", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<Result<Guid>>();
        result.Should().NotBeNull();
        result!.Succeeded.Should().BeTrue();
        result.Data.Should().NotBe(Guid.Empty);
    }

    [Test]
    public async Task CreateMessage_WithBookingId_ShouldReturnSuccess()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();

        var guest = new Guest
        {
            Id = guestId,
            FirstName = "Rikardo",
            LastName = "Persson",
            CreatedAt = DateTime.UtcNow,
        };
        var booking = new Booking
        {
            Id = bookingId,
            GuestId = guestId,
            CheckIn = DateTime.UtcNow.AddDays(-1),
            CheckOut = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
        };

        _context.Guests.Add(guest);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        var command = new CreateMessageCommand
        {
            BookingId = bookingId,
            Content = "Ditt flyg åker om 2 timmar",
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Messages", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<Result<Guid>>();
        result!.Succeeded.Should().BeTrue();
        result.Data.Should().NotBe(Guid.Empty);
    }

    [Test]
    public async Task CreateMessage_WithUnknownIds_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new CreateMessageCommand
        {
            HotelId = Guid.NewGuid(),
            BookingId = Guid.NewGuid(),
            Content = "Hej",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Messages", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<Result<Guid>>();
        result.Should().NotBeNull();
        result!.Succeeded.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Contains("Hotel med angivet ID finns inte"));
    }

}
