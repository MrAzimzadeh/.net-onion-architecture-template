using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChangesV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Policies",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Group",
                table: "Policies",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Policies",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Policies",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "Policies",
                columns: new[] { "Id", "CreatedBy", "CreationAt", "Description", "DisplayName", "Group", "IsActive", "LastModifiedAt", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("01a8ea06-74f9-e74d-bae7-dac9ab103c15"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Tenants.Update", "Update", "Tenants", true, null, null, "Policies.Tenants.Update" },
                    { new Guid("08ab7303-9b66-9047-bad1-60193b07d025"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Tenants.List", "List", "Tenants", true, null, null, "Policies.Tenants.List" },
                    { new Guid("2a077084-536c-3d44-914c-65adc0acc523"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Tenants.Create", "Create", "Tenants", true, null, null, "Policies.Tenants.Create" },
                    { new Guid("2beb0e5f-415c-6042-8fe2-9614a1a1aac4"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Users.Update", "Update", "Users", true, null, null, "Policies.Users.Update" },
                    { new Guid("3d990739-98f0-f047-8796-fb1770b5c803"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Users.Create", "Create", "Users", true, null, null, "Policies.Users.Create" },
                    { new Guid("3e9a80b2-3ec5-a546-a4cb-dd13f5601251"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Tenants.Delete", "Delete", "Tenants", true, null, null, "Policies.Tenants.Delete" },
                    { new Guid("7919fc92-643b-2d45-adac-4dd5c5b45837"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Users.View", "View", "Users", true, null, null, "Policies.Users.View" },
                    { new Guid("b4f8a89a-6c23-344c-8504-da60ae9d22b6"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Users.Delete", "Delete", "Users", true, null, null, "Policies.Users.Delete" },
                    { new Guid("ca24d9f8-1326-c34b-ba86-d8d969823e8c"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Tenants.View", "View", "Tenants", true, null, null, "Policies.Tenants.View" },
                    { new Guid("f63bc3af-aee8-5d4b-8b32-00b7b5d90c01"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permission for Policies.Users.List", "List", "Users", true, null, null, "Policies.Users.List" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Policies_Name",
                table: "Policies",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Policies_Name",
                table: "Policies");

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("01a8ea06-74f9-e74d-bae7-dac9ab103c15"));

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("08ab7303-9b66-9047-bad1-60193b07d025"));

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("2a077084-536c-3d44-914c-65adc0acc523"));

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("2beb0e5f-415c-6042-8fe2-9614a1a1aac4"));

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("3d990739-98f0-f047-8796-fb1770b5c803"));

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("3e9a80b2-3ec5-a546-a4cb-dd13f5601251"));

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("7919fc92-643b-2d45-adac-4dd5c5b45837"));

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("b4f8a89a-6c23-344c-8504-da60ae9d22b6"));

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("ca24d9f8-1326-c34b-ba86-d8d969823e8c"));

            migrationBuilder.DeleteData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("f63bc3af-aee8-5d4b-8b32-00b7b5d90c01"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Policies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Group",
                table: "Policies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Policies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Policies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }
    }
}
