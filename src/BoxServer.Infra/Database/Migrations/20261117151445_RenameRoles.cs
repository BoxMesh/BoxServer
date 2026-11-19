using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoxServer.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"User\" SET \"Role\" = CASE \"Role\" WHEN 'Admin' THEN 'System' WHEN 'Operator' THEN 'User' ELSE \"Role\" END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"User\" SET \"Role\" = CASE \"Role\" WHEN 'System' THEN 'Admin' WHEN 'User' THEN 'Operator' ELSE \"Role\" END");
        }
    }
}
