using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammaGo.Server.Migrations
{
    /// <inheritdoc />
    public partial class ExerciseResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccuracyPercentage",
                table: "ExerciseResults",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CorrectCount",
                table: "ExerciseResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IncorrectCount",
                table: "ExerciseResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseResults_ExerciseId",
                table: "ExerciseResults",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseResults_UserId",
                table: "ExerciseResults",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseResults_Exercises_ExerciseId",
                table: "ExerciseResults",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseResults_Usuarios_UserId",
                table: "ExerciseResults",
                column: "UserId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseResults_Exercises_ExerciseId",
                table: "ExerciseResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseResults_Usuarios_UserId",
                table: "ExerciseResults");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseResults_ExerciseId",
                table: "ExerciseResults");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseResults_UserId",
                table: "ExerciseResults");

            migrationBuilder.DropColumn(
                name: "AccuracyPercentage",
                table: "ExerciseResults");

            migrationBuilder.DropColumn(
                name: "CorrectCount",
                table: "ExerciseResults");

            migrationBuilder.DropColumn(
                name: "IncorrectCount",
                table: "ExerciseResults");
        }
    }
}
