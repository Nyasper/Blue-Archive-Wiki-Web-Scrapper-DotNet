using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlueArchiveWebScrapper.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    role = table.Column<string>(type: "TEXT", nullable: false),
                    combatClass = table.Column<string>(type: "TEXT", nullable: false),
                    weaponType = table.Column<string>(type: "TEXT", nullable: false),
                    age = table.Column<int>(type: "INTEGER", nullable: true),
                    birthday = table.Column<string>(type: "TEXT", nullable: true),
                    height = table.Column<int>(type: "INTEGER", nullable: true),
                    hobbies = table.Column<string>(type: "TEXT", nullable: true),
                    designer = table.Column<string>(type: "TEXT", nullable: true),
                    illustrator = table.Column<string>(type: "TEXT", nullable: true),
                    voice = table.Column<string>(type: "TEXT", nullable: false),
                    releaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    skinSet = table.Column<string>(type: "TEXT", nullable: false),
                    pageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    pageImageProfileUrl = table.Column<string>(type: "TEXT", nullable: false),
                    pageImageFullUrl = table.Column<string>(type: "TEXT", nullable: false),
                    audioUrl = table.Column<string>(type: "TEXT", nullable: false),
                    files = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.charaName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "students");
        }
    }
}
