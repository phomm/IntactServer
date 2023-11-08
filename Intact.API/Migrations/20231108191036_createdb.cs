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
                    Id = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    TermName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, computedColumnSql: "\"Id\" || 'FactionName'", stored: true),
                    TermDescription = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, computedColumnSql: "\"Id\" || 'FactionDescription'", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Localizations",
                columns: table => new
                {
                    TermId = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Value = table.Column<string>(type: "character varying(950)", maxLength: 950, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizations", x => new { x.TermId, x.LanguageCode });
                });

            migrationBuilder.CreateTable(
                name: "MapBuildings",
                columns: table => new
                {
                    MapId = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Proto = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    X = table.Column<int>(type: "integer", nullable: false),
                    Y = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapBuildings", x => new { x.MapId, x.Number });
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", maxLength: 32, nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Factions = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SceneBackground = table.Column<string>(type: "text", nullable: false),
                    TermName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, computedColumnSql: "\"Id\" || 'MapName'", stored: true),
                    TermDescription = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, computedColumnSql: "\"Id\" || 'MapDescription'", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => new { x.Id, x.Version });
                });

            migrationBuilder.CreateTable(
                name: "PlayerOptions",
                columns: table => new
                {
                    MapId = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Money = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    Left = table.Column<int>(type: "integer", nullable: false),
                    Right = table.Column<int>(type: "integer", nullable: false),
                    Top = table.Column<int>(type: "integer", nullable: false),
                    Bottom = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerOptions", x => new { x.MapId, x.Number });
                });

            migrationBuilder.CreateTable(
                name: "ProtoBuildings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    AssetId = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    InLife = table.Column<byte>(type: "smallint", nullable: false),
                    BuildingType = table.Column<int>(type: "integer", nullable: false),
                    TermName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, computedColumnSql: "\"Id\" || 'BuildingName'", stored: true),
                    TermDescription = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, computedColumnSql: "\"Id\" || 'BuildingDescription'", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtoBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProtoWarriors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    FactionId = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Force = table.Column<int>(type: "integer", nullable: false),
                    AssetId = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    IsHero = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsRanged = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsMelee = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsBlockFree = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsImmune = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    InLife = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)1),
                    InMana = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    InMoves = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)1),
                    InActs = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)1),
                    InShots = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    Cost = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    TermName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, computedColumnSql: "\"Id\" || 'WarriorName'", stored: true),
                    TermDescription = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, computedColumnSql: "\"Id\" || 'WarriorDescription'", stored: true)
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
                name: "MapBuildings");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "PlayerOptions");

            migrationBuilder.DropTable(
                name: "ProtoBuildings");

            migrationBuilder.DropTable(
                name: "ProtoWarriors");
        }
    }
}
