using DashyBoard.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DashyBoard.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Hotel> Hotels { get; }
    DbSet<Room> Rooms { get; }
    DbSet<Guest> Guests { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<Flight> Flights { get; }
    DbSet<Admin> Admins { get; }
    DbSet<Message> Messages { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
