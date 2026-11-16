using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BoxServer.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class DeviceMetric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceMetric",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Cpu = table.Column<double>(type: "double precision", nullable: false),
                    Ram = table.Column<double>(type: "double precision", nullable: false),
                    Disk = table.Column<double>(type: "double precision", nullable: false),
                    NetIn = table.Column<double>(type: "double precision", nullable: false),
                    NetOut = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMetric", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceMetric_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMetric_DeviceId_At",
                table: "DeviceMetric",
                columns: new[] { "DeviceId", "At" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceMetric");
        }
    }
}
