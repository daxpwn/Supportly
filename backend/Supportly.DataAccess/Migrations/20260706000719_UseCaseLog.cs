using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Supportly.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UseCaseLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UseCaseLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UseCaseId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UseCaseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExecutedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    DurationMs = table.Column<long>(type: "bigint", nullable: false),
                    Succeeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseCaseLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UseCaseLogs_ExecutedAt",
                table: "UseCaseLogs",
                column: "ExecutedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UseCaseLogs_UseCase",
                table: "UseCaseLogs",
                column: "UseCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UseCaseLogs_User",
                table: "UseCaseLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UseCaseLogs");
        }
    }
}
