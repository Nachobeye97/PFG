using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammaGo.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseResultsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "ExerciseId",
                table: "ExerciseResults");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "ExerciseResults");

            migrationBuilder.AddColumn<int>(
                name: "AttemptNumber",
                table: "ExerciseResults",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttemptNumber",
                table: "ExerciseResults");

            migrationBuilder.AddColumn<decimal>(
                name: "AccuracyPercentage",
                table: "ExerciseResults",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "ExerciseId",
                table: "ExerciseResults",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "ExerciseResults",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
    }
}
