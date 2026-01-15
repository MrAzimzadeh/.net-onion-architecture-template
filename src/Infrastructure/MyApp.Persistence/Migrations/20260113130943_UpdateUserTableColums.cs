using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTableColums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "firstname",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "password_salt",
                table: "Users",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "last_modified_by",
                table: "Users",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "last_modified_at",
                table: "Users",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "email_address",
                table: "Users",
                newName: "EmailAddress");

            migrationBuilder.RenameColumn(
                name: "creation_at",
                table: "Users",
                newName: "CreationAt");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Users",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "uk_users_email_address",
                table: "Users",
                newName: "EmailAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "firstname");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "password_salt");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Users",
                newName: "last_modified_by");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "Users",
                newName: "last_modified_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Users",
                newName: "email_address");

            migrationBuilder.RenameColumn(
                name: "CreationAt",
                table: "Users",
                newName: "creation_at");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Users",
                newName: "created_by");

            migrationBuilder.RenameIndex(
                name: "EmailAddress",
                table: "Users",
                newName: "uk_users_email_address");
        }
    }
}
