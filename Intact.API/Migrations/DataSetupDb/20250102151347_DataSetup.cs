using Intact.API.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intact.API.Migrations.DataSetupDb
{
    /// <inheritdoc />
    public partial class DataSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFile(filesPath: "Scripts", fileName: "content_default_pg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
