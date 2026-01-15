using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure.Seeders
{
    public class UserSeeder : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            Guid userId = Guid.Parse("017f22e6-bb67-7e17-8a60-9f822a22a3e1");
            DateTime date = DateTime.SpecifyKind(DateTime.Parse("2024-10-15 03:03:39"), DateTimeKind.Utc);

            builder.HasData(new List<User>
            {
                new()
                {
                    Id = userId,
                    FirstName = "Mahammad",
                    LastName = "Azimzada",
                    PhoneNumber = "+994509888128",
                    EmailAddress = "mahammad.azimzada@gmail.com",
                    IsActive = true,
                    PasswordHash = "cwnM71bM4LdWaVs/1dUvaOQHrZh4ZCli3XX1TEK3UDuDNYG9GLPvEFefUSP3mi/rcEbniYMhIXt44lRLfheUnA==",
                    PasswordSalt = "3ug03IvjXIHkzVGAk+x/TZ4nAophlLeJcaWBWzpzWsPSKYOWDsBi9eU9C0/09Vemza7GFL3d1NaRKUJelhYqNrCjgPFrBQGGYCJZK71ST+xSWCCem/MaXzoDGY3soMlKc6L+89D6squSOdnQYieYklDvXR+hEeEt9U0X+c3SqFs=",
                    CreatedBy = userId,
                    CreationAt = date
                }
            });
        }
    }
}