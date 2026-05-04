using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashyBoard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPilotToGuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPilot",
                table: "Guests",
                type: "INTEGER",
                nullable: false,
                defaultValue: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "IsPilot", table: "Guests");
        }
    }
}
