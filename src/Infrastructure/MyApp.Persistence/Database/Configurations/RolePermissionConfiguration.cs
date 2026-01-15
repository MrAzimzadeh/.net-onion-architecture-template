using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Application.Security;
using MyApp.Domain.Entities;

namespace MyApp.Persistence.Database.Configurations
{
    public class RolePolicyConfiguration : IEntityTypeConfiguration<RolePolicy>
    {
        public void Configure(EntityTypeBuilder<RolePolicy> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.RolePolicies)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Policy)
                .WithMany()
                .HasForeignKey(x => x.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.RoleId, x.PolicyId }).IsUnique();

            // Seed data disabled - RolePolicy relationships should be managed through application code
            // to ensure Policy entities exist before creating references to them
        }
    }
}
