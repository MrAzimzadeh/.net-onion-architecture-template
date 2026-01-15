using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Application.Security;
using MyApp.Domain.Entities;

namespace MyApp.Persistence.Seeders
{
    public class PolicySeeder : IEntityTypeConfiguration<Policy>
    {
        // Static date to ensure consistency
        private static readonly DateTime SeedDate = DateTime.SpecifyKind(DateTime.Parse("2024-10-15 03:03:39"), DateTimeKind.Utc);

        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            var policies = new List<Policy>();

            // Use reflection to iterate through DefaultPermissions
            var groups = typeof(Policies).GetNestedTypes();
            foreach (var group in groups)
            {
                var groupName = group.GetField("Group")?.GetValue(null)?.ToString();
                if (string.IsNullOrEmpty(groupName)) continue;

                var permissionFields = group.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(f => f.FieldType == typeof(string) && f.Name != "Group");

                foreach (var field in permissionFields)
                {
                    var permissionName = field.GetValue(null)?.ToString();
                    if (string.IsNullOrEmpty(permissionName)) continue;

                    policies.Add(new Policy
                    {
                        Id = GenerateDeterministicGuid(permissionName),
                        Name = permissionName,
                        DisplayName = permissionName.Split('.').Last(), // Example: "Create" from "Product.Create"
                        Group = groupName,
                        CreationAt = SeedDate,
                        CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        LastModifiedAt = SeedDate, // Initially same as CreatedAt
                        Description = $"Permission for {permissionName}" // Optional description
                    });
                }
            }

            // Seed permissions into the database
            builder.HasData(policies);
        }

        /// <summary>
        /// Generates a deterministic GUID based on the input string
        /// This ensures the same permission name always generates the same GUID
        /// </summary>
        public static Guid GenerateDeterministicGuid(string input)
        {
            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var guidBytes = new byte[16];
            Array.Copy(hash, 0, guidBytes, 0, 16);

            // Set version (4) and variant bits according to RFC 4122
            guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x40); // Version 4
            guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80); // Variant bits

            return new Guid(guidBytes);
        }
    }
}