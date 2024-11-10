using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammaGo.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateExercisesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Exercises",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exercises");
        }
    }
}
