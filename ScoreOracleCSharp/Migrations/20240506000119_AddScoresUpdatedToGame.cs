using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreOracleCSharp.Migrations
{
    /// <inheritdoc />
    public partial class AddScoresUpdatedToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ScoresUpdated",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoresUpdated",
                table: "Games");
        }
    }
}
