using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammaGo.Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameDateCompletedToDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCompleted",
                table: "ExerciseResults",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ExerciseResults",
                newName: "DateCompleted");
        }
    }
}
