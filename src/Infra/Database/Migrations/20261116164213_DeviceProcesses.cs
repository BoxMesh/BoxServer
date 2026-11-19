using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zin.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class DeviceProcesses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Processes",
                table: "Device",
                type: "jsonb",
                nullable: false,
                // An empty array, not EF's generated "" (which Postgres stores as the object {}).
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processes",
                table: "Device");
        }
    }
}
