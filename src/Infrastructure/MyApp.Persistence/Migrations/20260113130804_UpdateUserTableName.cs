using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDeviceSessions_users_UserId",
                table: "UserDeviceSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExternalLogins_users_UserId",
                table: "UserExternalLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_users_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_users_Tenants_TenantId",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_users_TenantId",
                table: "Users",
                newName: "IX_Users_TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeviceSessions_Users_UserId",
                table: "UserDeviceSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExternalLogins_Users_UserId",
                table: "UserExternalLogins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDeviceSessions_Users_UserId",
                table: "UserDeviceSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExternalLogins_Users_UserId",
                table: "UserExternalLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameIndex(
                name: "IX_Users_TenantId",
                table: "users",
                newName: "IX_users_TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeviceSessions_users_UserId",
                table: "UserDeviceSessions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExternalLogins_users_UserId",
                table: "UserExternalLogins",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_Tenants_TenantId",
                table: "users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }
    }
}
