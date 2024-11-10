using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrammaGo.Server.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureIdentityForUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExerciseResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ExerciseId = table.Column<long>(type: "bigint", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseResults");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Usuarios");
        }
    }
}
