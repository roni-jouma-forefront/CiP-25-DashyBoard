using System.Reflection;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Domain.Common;
using DashyBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DashyBoard.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private static readonly TimeZoneInfo SwedenTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
        "W. Europe Standard Time"
    );

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Convert all Guid properties to TEXT for Turso/LibSQL compatibility
        var guidToStringConverter = new ValueConverter<Guid, string>(
            v => v.ToString("D"),
            v => Guid.Parse(v)
        );
        var nullableGuidToStringConverter = new ValueConverter<Guid?, string?>(
            v => v.HasValue ? v.Value.ToString("D") : null,
            v => v != null ? Guid.Parse(v) : null
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(Guid))
                {
                    property.SetValueConverter(guidToStringConverter);
                }
                else if (property.ClrType == typeof(Guid?))
                {
                    property.SetValueConverter(nullableGuidToStringConverter);
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;
        var swedenNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, SwedenTimeZone);
        var truncatedNow = new DateTime(
            swedenNow.Year,
            swedenNow.Month,
            swedenNow.Day,
            swedenNow.Hour,
            swedenNow.Minute,
            swedenNow.Second,
            DateTimeKind.Unspecified
        );

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = truncatedNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = truncatedNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
