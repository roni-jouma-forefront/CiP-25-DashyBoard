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
        var hotel = new Hotel
        {
            Id = 1,
            Name = "Test Hotel",
            CreatedAt = DateTime.UtcNow,
        };
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        var command = new CreateMessageCommand { HotelId = 1, Content = "Frukosten st�nger 10:00" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Messages", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<Result<int>>();
        result.Should().NotBeNull();
        result!.Succeeded.Should().BeTrue();
        result.Data.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task CreateMessage_WithBookingId_ShouldReturnSuccess()
    {
        // Arrange
        var guest = new Guest
        {
            Id = 1,
            FullName = "Rikardo",
            CreatedAt = DateTime.UtcNow,
        };
        var booking = new Booking
        {
            Id = 1,
            GuestId = 1,
            CheckIn = DateTime.UtcNow.AddDays(-1),
            CheckOut = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
        };

        _context.Guests.Add(guest);
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        var command = new CreateMessageCommand
        {
            BookingId = 1,
            Content = "Ditt flyg �ker om 2 timmar",
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Messages", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<Result<int>>();
        result!.Succeeded.Should().BeTrue();
        result.Data.Should().BeGreaterThan(0);
    }
}
