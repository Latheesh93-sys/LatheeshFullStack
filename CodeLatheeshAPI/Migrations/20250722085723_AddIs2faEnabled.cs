using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeLatheeshAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIs2faEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is2faEnabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is2faEnabled",
                table: "Users");
        }
    }
}
