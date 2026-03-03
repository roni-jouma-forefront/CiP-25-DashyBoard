using DashyBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashyBoard.Infrastructure.Persistence.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.Username).HasMaxLength(100);

        builder.Property(a => a.FullName).HasMaxLength(200);

        builder.Property(a => a.Role).HasMaxLength(50);

        builder
            .HasOne<Hotel>()
            .WithMany()
            .HasForeignKey(a => a.HotelId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
