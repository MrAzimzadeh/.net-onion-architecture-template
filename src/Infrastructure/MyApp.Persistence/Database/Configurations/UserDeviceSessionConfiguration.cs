using MyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApp.Persistence.Database.Configurations
{
    public class UserDeviceSessionConfiguration : IEntityTypeConfiguration<UserDeviceSession>
    {
        public void Configure(EntityTypeBuilder<UserDeviceSession> builder)
        {
            builder.ToTable("UserDeviceSessions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DeviceId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.RefreshToken)
                .HasMaxLength(256);

            builder.Property(x => x.DeviceType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.DeviceName)
                .HasMaxLength(100);

            // Create a unique index on UserId + DeviceId to prevent duplicate sessions
            builder.HasIndex(x => new { x.UserId, x.DeviceId })
                .IsUnique();

            // Add relationship
            builder.HasOne(x => x.User)
                .WithMany(x => x.DeviceSessions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
