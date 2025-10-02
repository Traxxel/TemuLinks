using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemuLinks.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TemuLinksInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 2, 10, 45, 30, 271, DateTimeKind.Utc).AddTicks(3570));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 2, 10, 37, 26, 832, DateTimeKind.Utc).AddTicks(8450));
        }
    }
}
