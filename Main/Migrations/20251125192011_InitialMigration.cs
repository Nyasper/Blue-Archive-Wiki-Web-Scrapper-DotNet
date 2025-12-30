using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    charaName = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    lastName = table.Column<string>(type: "TEXT", nullable: false),
                    school = table.Column<string>(type: "TEXT", nullable: false),
                    age = table.Column<int>(type: "INTEGER", nullable: true),
                    height = table.Column<int>(type: "INTEGER", nullable: true),
                    birthday = table.Column<string>(type: "TEXT", nullable: true),
                    hobbies = table.Column<string>(type: "TEXT", nullable: true),
                    designer = table.Column<string>(type: "TEXT", nullable: true),
                    illustrator = table.Column<string>(type: "TEXT", nullable: true),
                    voice = table.Column<string>(type: "TEXT", nullable: false),
                    releaseDate = table.Column<string>(type: "TEXT", nullable: false),
                    skinSet = table.Column<string>(type: "TEXT", nullable: false),
                    pageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    imageProfileUrl = table.Column<string>(type: "TEXT", nullable: false),
                    imageFullUrl = table.Column<string>(type: "TEXT", nullable: false),
                    smallImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    audioUrl = table.Column<string>(type: "TEXT", nullable: false),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.charaName);
                });

            migrationBuilder.CreateIndex(
                name: "IX_students_charaName",
                table: "students",
                column: "charaName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "students");
        }
    }
}
