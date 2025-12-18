using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fileManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class EdPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "PersonalDocuments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "PersonalDocuments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
