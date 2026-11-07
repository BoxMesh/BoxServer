using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoxServer.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class DeviceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Hostname = table.Column<string>(type: "text", nullable: false),
                    OSName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OSVersion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OSArchitecture = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IPAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    MacAddress = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    CPU = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RAM = table.Column<long>(type: "bigint", nullable: false),
                    Disk = table.Column<long>(type: "bigint", nullable: false),
                    Uptime = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Device");
        }
    }
}
