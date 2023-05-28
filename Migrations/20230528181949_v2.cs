using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppDiplomNew.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_lesson",
                table: "Informs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name_lesson",
                table: "Informs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
