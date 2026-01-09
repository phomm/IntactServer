using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intact.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCommandsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    QueueNumber = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<int>(type: "integer", nullable: false),
                    PlayerIndex = table.Column<int>(type: "integer", nullable: false),
                    CommandId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Error = table.Column<short>(type: "smallint", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => new { x.RoomId, x.QueueNumber });
                    table.ForeignKey(
                        name: "FK_Commands_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commands_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commands_ProfileId",
                table: "Commands",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commands");
        }
    }
}
