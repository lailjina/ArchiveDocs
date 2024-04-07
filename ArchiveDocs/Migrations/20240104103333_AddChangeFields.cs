using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArchiveDocs.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Objects",
                newName: "StatusObject");

            migrationBuilder.RenameColumn(
                name: "ObjectId",
                table: "Objects",
                newName: "Id_Object");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Documents",
                newName: "StatusDoc");

            migrationBuilder.RenameColumn(
                name: "FileHash",
                table: "Documents",
                newName: "HashSum");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "Documents",
                newName: "Id_Doc");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateArchived",
                table: "Objects",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "HranitDo",
                table: "Documents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateArchived",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "HranitDo",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "StatusObject",
                table: "Objects",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Id_Object",
                table: "Objects",
                newName: "ObjectId");

            migrationBuilder.RenameColumn(
                name: "StatusDoc",
                table: "Documents",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "HashSum",
                table: "Documents",
                newName: "FileHash");

            migrationBuilder.RenameColumn(
                name: "Id_Doc",
                table: "Documents",
                newName: "DocumentId");
        }
    }
}
