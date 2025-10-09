using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemuLinks.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailUniqueAndMakeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop unique index on Email if exists
            try
            {
                migrationBuilder.DropIndex(
                    name: "IX_Users_Email",
                    table: "Users");
            }
            catch {}

            // Make Email nullable
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert Email to NOT NULL with default empty string
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            // Recreate unique index on Email
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }
    }
}


