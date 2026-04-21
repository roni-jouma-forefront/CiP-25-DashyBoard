using DashyBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DashyBoard.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.RecurrenceType)
            .HasDefaultValue("None")
            .IsRequired();
        builder.Property(m => m.PostedBy).HasMaxLength(100);

        builder.Property(m => m.RecurrenceTimeStart)
            .HasConversion(
                t => t.HasValue ? t.Value.ToString("HH:mm:ss") : null,
                s => s != null ? TimeOnly.Parse(s) : (TimeOnly?)null
            );

        builder.Property(m => m.RecurrenceTimeEnd)
            .HasConversion(
                t => t.HasValue ? t.Value.ToString("HH:mm:ss") : null,
                s => s != null ? TimeOnly.Parse(s) : (TimeOnly?)null
            );

        builder.Property(m => m.Title).HasMaxLength(200);

        builder.Property(m => m.Content).HasMaxLength(500);

        builder
            .HasOne<Hotel>()
            .WithMany()
            .HasForeignKey(m => m.HotelId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne<Booking>()
            .WithMany()
            .HasForeignKey(m => m.BookingId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
