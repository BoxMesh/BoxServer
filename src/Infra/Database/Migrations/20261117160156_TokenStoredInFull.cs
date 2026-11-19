using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zin.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class TokenStoredInFull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Old tokens kept only a hash, so their value can no longer be shown or copied: drop them.
            migrationBuilder.Sql("DELETE FROM \"EnrollmentToken\"");

            migrationBuilder.DropColumn(
                name: "Tail",
                table: "EnrollmentToken");

            migrationBuilder.RenameColumn(
                name: "TokenHash",
                table: "EnrollmentToken",
                newName: "Token");

            migrationBuilder.RenameIndex(
                name: "IX_EnrollmentToken_TokenHash",
                table: "EnrollmentToken",
                newName: "IX_EnrollmentToken_Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "EnrollmentToken",
                newName: "TokenHash");

            migrationBuilder.RenameIndex(
                name: "IX_EnrollmentToken_Token",
                table: "EnrollmentToken",
                newName: "IX_EnrollmentToken_TokenHash");

            migrationBuilder.AddColumn<string>(
                name: "Tail",
                table: "EnrollmentToken",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }
    }
}
