using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoxServer.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class DeviceUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgentVersion",
                table: "Device",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "CpuUsage",
                table: "Device",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DiskUsage",
                table: "Device",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NetIn",
                table: "Device",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NetOut",
                table: "Device",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RamUsage",
                table: "Device",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentVersion",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "CpuUsage",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "DiskUsage",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "NetIn",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "NetOut",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "RamUsage",
                table: "Device");
        }
    }
}
