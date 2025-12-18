using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fileManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class EditDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalDocument_PersonalData_PersonalDataId",
                table: "PersonalDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalDocument",
                table: "PersonalDocument");

            migrationBuilder.RenameTable(
                name: "PersonalDocument",
                newName: "PersonalDocuments");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalDocument_PersonalDataId",
                table: "PersonalDocuments",
                newName: "IX_PersonalDocuments_PersonalDataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalDocuments",
                table: "PersonalDocuments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalDocuments_PersonalData_PersonalDataId",
                table: "PersonalDocuments",
                column: "PersonalDataId",
                principalTable: "PersonalData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalDocuments_PersonalData_PersonalDataId",
                table: "PersonalDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalDocuments",
                table: "PersonalDocuments");

            migrationBuilder.RenameTable(
                name: "PersonalDocuments",
                newName: "PersonalDocument");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalDocuments_PersonalDataId",
                table: "PersonalDocument",
                newName: "IX_PersonalDocument_PersonalDataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalDocument",
                table: "PersonalDocument",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalDocument_PersonalData_PersonalDataId",
                table: "PersonalDocument",
                column: "PersonalDataId",
                principalTable: "PersonalData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
