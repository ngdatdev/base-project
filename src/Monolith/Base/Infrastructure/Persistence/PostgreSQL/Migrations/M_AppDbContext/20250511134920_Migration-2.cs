using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.PostgreSQL.Migrations.M_AppDbContext
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "UserTokens",
                schema: "auths",
                comment: "Contain back up code records.");

            migrationBuilder.AlterTable(
                name: "UserLogins",
                schema: "auths",
                comment: "Contain UserLogin records.");

            migrationBuilder.AlterTable(
                name: "UserClaims",
                schema: "auths",
                comment: "Contain UserClaim records.");

            migrationBuilder.AlterTable(
                name: "RoleClaims",
                schema: "auths",
                comment: "Contain RoleClaim records.");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "auths",
                table: "UserTokens",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                schema: "auths",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                schema: "auths",
                table: "UserTokens",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "auths",
                table: "UserLogins",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "auths",
                table: "UserClaims",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "auths",
                table: "RoleClaims",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "auths",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auths",
                        principalTable: "Users",
                        principalColumn: "Id");
                },
                comment: "Contain AuditLog code records.");

            migrationBuilder.CreateTable(
                name: "UserAuthProviders",
                schema: "auths",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthProviders", x => x.Id);
                },
                comment: "Contain UserAuthProvider records.");

            migrationBuilder.CreateTable(
                name: "UserDevices",
                schema: "auths",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: true),
                    DeviceType = table.Column<string>(type: "text", nullable: true),
                    DeviceName = table.Column<string>(type: "text", nullable: true),
                    LastUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDevices_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auths",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Contain back up code records.");

            migrationBuilder.CreateTable(
                name: "UserSessions",
                schema: "auths",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExperiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    DeviderId = table.Column<string>(type: "text", nullable: true),
                    IpAddress = table.Column<string>(type: "text", nullable: true),
                    LastActive = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSessions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auths",
                        principalTable: "Users",
                        principalColumn: "Id");
                },
                comment: "Contain back up code records.");

            migrationBuilder.CreateTable(
                name: "UserTwoFactors",
                schema: "auths",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SecretKey = table.Column<string>(type: "text", nullable: true),
                    TwoFAMethod = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTwoFactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTwoFactors_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auths",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Contain UserTwoFactor records.");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId1",
                schema: "auths",
                table: "UserTokens",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                schema: "auths",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_UserId",
                schema: "auths",
                table: "UserDevices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                schema: "auths",
                table: "UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTwoFactors_UserId",
                schema: "auths",
                table: "UserTwoFactors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Users_UserId1",
                schema: "auths",
                table: "UserTokens",
                column: "UserId1",
                principalSchema: "auths",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Users_UserId1",
                schema: "auths",
                table: "UserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs",
                schema: "auths");

            migrationBuilder.DropTable(
                name: "UserAuthProviders",
                schema: "auths");

            migrationBuilder.DropTable(
                name: "UserDevices",
                schema: "auths");

            migrationBuilder.DropTable(
                name: "UserSessions",
                schema: "auths");

            migrationBuilder.DropTable(
                name: "UserTwoFactors",
                schema: "auths");

            migrationBuilder.DropIndex(
                name: "IX_UserTokens_UserId1",
                schema: "auths",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "auths",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                schema: "auths",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "auths",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "auths",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "auths",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "auths",
                table: "RoleClaims");

            migrationBuilder.AlterTable(
                name: "UserTokens",
                schema: "auths",
                oldComment: "Contain back up code records.");

            migrationBuilder.AlterTable(
                name: "UserLogins",
                schema: "auths",
                oldComment: "Contain UserLogin records.");

            migrationBuilder.AlterTable(
                name: "UserClaims",
                schema: "auths",
                oldComment: "Contain UserClaim records.");

            migrationBuilder.AlterTable(
                name: "RoleClaims",
                schema: "auths",
                oldComment: "Contain RoleClaim records.");
        }
    }
}
