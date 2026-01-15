using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Entities;

namespace MyApp.Persistence.Database.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(r => r.RolePolicies)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed default Admin role
            builder.HasData(new Role
            {
                Id = Guid.Parse("308660dc-ae51-480f-824d-7dca6714c3e2"),
                Name = "Admin",
                Description = "Administrator role with full access",
                CreatedBy = Guid.Empty,

            });
        }
    }
}
