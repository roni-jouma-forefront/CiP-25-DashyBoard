using DashyBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashyBoard.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id).ValueGeneratedOnAdd();

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
