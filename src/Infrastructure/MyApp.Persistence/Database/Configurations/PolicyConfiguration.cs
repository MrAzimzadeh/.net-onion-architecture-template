using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Application.Security;
using MyApp.Domain.Entities;
using MyApp.Persistence.Seeders;

namespace MyApp.Persistence.Database.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Group).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);

            builder.HasIndex(x => x.Name).IsUnique();

            // Get all permissions from the Permissions class
            var policies = typeof(Policies)
                .GetNestedTypes()
                .SelectMany(t => t.GetFields())
                .Where(f => f.IsStatic && f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .Select(f => f.GetValue(null)?.ToString())
                .Where(p => p != null)
                .ToList();

            var seedData = policies.Select(p => new Policy
            {
                Id = PolicySeeder.GenerateDeterministicGuid(p!),
                Name = p!,
                DisplayName = p!.Split('.').Last(),
                Group = p!.Split('.').Skip(1).First(),
                Description = $"Permission for {p}",
                CreationAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                CreatedBy = Guid.Empty
            });

            builder.HasData(seedData);
        }
    }
}
