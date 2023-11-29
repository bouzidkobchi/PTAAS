using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class fixingastrangeissue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PentestingMethodologyPentrationTest_PentestingMethodology_MethodologiesName",
                table: "PentestingMethodologyPentrationTest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PentestingMethodology",
                table: "PentestingMethodology");

            migrationBuilder.RenameTable(
                name: "PentestingMethodology",
                newName: "PentestingMethodologies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PentestingMethodologies",
                table: "PentestingMethodologies",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_PentestingMethodologyPentrationTest_PentestingMethodologies_MethodologiesName",
                table: "PentestingMethodologyPentrationTest",
                column: "MethodologiesName",
                principalTable: "PentestingMethodologies",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PentestingMethodologyPentrationTest_PentestingMethodologies_MethodologiesName",
                table: "PentestingMethodologyPentrationTest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PentestingMethodologies",
                table: "PentestingMethodologies");

            migrationBuilder.RenameTable(
                name: "PentestingMethodologies",
                newName: "PentestingMethodology");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PentestingMethodology",
                table: "PentestingMethodology",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_PentestingMethodologyPentrationTest_PentestingMethodology_MethodologiesName",
                table: "PentestingMethodologyPentrationTest",
                column: "MethodologiesName",
                principalTable: "PentestingMethodology",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
