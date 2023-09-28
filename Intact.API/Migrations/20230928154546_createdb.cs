using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intact.API.Migrations
{
    public partial class createdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TermName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, computedColumnSql: "[Id] + 'FactionName'"),
                    TermDescription = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, computedColumnSql: "[Id] + 'FactionDescription'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Localizations",
                columns: table => new
                {
                    TermId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(950)", maxLength: 950, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizations", x => new { x.TermId, x.LanguageCode });
                });

            migrationBuilder.CreateTable(
                name: "ProtoBuildings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TermName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, computedColumnSql: "[Id] + 'BuildingName'"),
                    TermDescription = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, computedColumnSql: "[Id] + 'BuildingDescription'"),
                    AssetId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    InLife = table.Column<byte>(type: "tinyint", nullable: false),
                    BuildingType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtoBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProtoWarriors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TermName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, computedColumnSql: "[Id] + 'WarriorName'"),
                    TermDescription = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, computedColumnSql: "[Id] + 'WarriorDescription'"),
                    FactionId = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Force = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsHero = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsRanged = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsMelee = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsBlockFree = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsImmune = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    InLife = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    InMana = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    InMoves = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    InActs = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    InShots = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    Cost = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtoWarriors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Factions");

            migrationBuilder.DropTable(
                name: "Localizations");

            migrationBuilder.DropTable(
                name: "ProtoBuildings");

            migrationBuilder.DropTable(
                name: "ProtoWarriors");
        }
    }
}
