using System;
using System.Collections.Generic;
using BoilerPlate.Data.Domain.ValueObjects.System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoilerPlate.Data.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    language_code = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    security_questions = table.Column<IEnumerable<UserSecurityQuestion>>(type: "jsonb", nullable: false),
                    access_token_id = table.Column<Guid>(type: "uuid", nullable: true),
                    access_token_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    refresh_token_id = table.Column<Guid>(type: "uuid", nullable: true),
                    refresh_token_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_users_created_by",
                        column: x => x.created_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_users_deleted_by",
                        column: x => x.deleted_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_users_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "stored_files",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stored_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_stored_files_users_created_by",
                        column: x => x.created_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stored_files_users_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_stored_files_created_by",
                schema: "public",
                table: "stored_files",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_stored_files_updated_by",
                schema: "public",
                table: "stored_files",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_users_created_by",
                schema: "public",
                table: "users",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_users_deleted_by",
                schema: "public",
                table: "users",
                column: "deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                schema: "public",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_updated_by",
                schema: "public",
                table: "users",
                column: "updated_by");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stored_files",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}
