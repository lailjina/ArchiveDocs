using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArchiveDocs.Migrations
{
    /// <inheritdoc />
    public partial class AddNullValuesDocs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Objects_ObjectId",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "ObjectId",
                table: "Documents",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "HashSum",
                table: "Documents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Objects_ObjectId",
                table: "Documents",
                column: "ObjectId",
                principalTable: "Objects",
                principalColumn: "Id_Object");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Objects_ObjectId",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "ObjectId",
                table: "Documents",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HashSum",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Objects_ObjectId",
                table: "Documents",
                column: "ObjectId",
                principalTable: "Objects",
                principalColumn: "Id_Object",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
