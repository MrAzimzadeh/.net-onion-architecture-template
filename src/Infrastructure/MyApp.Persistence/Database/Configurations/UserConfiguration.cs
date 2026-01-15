using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Entities;

namespace MyApp.Persistence.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id)
                .HasName("pk_users");

            builder.HasIndex(e => e.EmailAddress)
                .IsUnique()
                .HasDatabaseName("EmailAddress");

            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .IsRequired();

            //
            // builder.Property(e => e.BranchId)
            //     .HasColumnName("branch_id");
            //
            // builder.HasOne(e => e.Branch)
            //     .WithMany(e => e.Users)
            //     .HasForeignKey(e => e.BranchId)
            //     .HasConstraintName("fk_users_branch_id")
            //     .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100);

            builder.Property(e => e.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100);

            builder.Property(e => e.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(100);

            builder.Property(e => e.EmailAddress)
                .HasColumnName("EmailAddress")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(e => e.PasswordSalt)
                .HasColumnName("PasswordSalt")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .HasColumnName("IsActive")
                .IsRequired();


            builder.Property(e => e.CreatedBy)
                .HasColumnName("CreatedBy")
                .IsRequired();

            builder.Property(e => e.CreationAt)
                .HasColumnName("CreationAt")
                .IsRequired();

            builder.Property(e => e.LastModifiedBy)
                .HasColumnName("LastModifiedBy");
            builder.Property(e => e.LastModifiedAt)
                .HasColumnName("LastModifiedAt");
        }
    }
}