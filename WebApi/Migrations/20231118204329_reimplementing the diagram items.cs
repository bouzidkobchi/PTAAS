using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class reimplementingthediagramitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PentestingMethodology",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PentestingMethodology", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TargetSystem",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetSystem", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "PentrationTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PentrationTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PentrationTest_Clients_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PentrationTest_TargetSystem_SystemId",
                        column: x => x.SystemId,
                        principalTable: "TargetSystem",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PentesterPentrationTest",
                columns: table => new
                {
                    PentestersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TokenTestsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PentesterPentrationTest", x => new { x.PentestersId, x.TokenTestsId });
                    table.ForeignKey(
                        name: "FK_PentesterPentrationTest_Pentesters_PentestersId",
                        column: x => x.PentestersId,
                        principalTable: "Pentesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PentesterPentrationTest_PentrationTest_TokenTestsId",
                        column: x => x.TokenTestsId,
                        principalTable: "PentrationTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PentestingMethodologyPentrationTest",
                columns: table => new
                {
                    MethodologyName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PentestingMethodologyPentrationTest", x => new { x.MethodologyName, x.TestsId });
                    table.ForeignKey(
                        name: "FK_PentestingMethodologyPentrationTest_PentestingMethodology_MethodologyName",
                        column: x => x.MethodologyName,
                        principalTable: "PentestingMethodology",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PentestingMethodologyPentrationTest_PentrationTest_TestsId",
                        column: x => x.TestsId,
                        principalTable: "PentrationTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PentesterPentrationTest_TokenTestsId",
                table: "PentesterPentrationTest",
                column: "TokenTestsId");

            migrationBuilder.CreateIndex(
                name: "IX_PentestingMethodologyPentrationTest_TestsId",
                table: "PentestingMethodologyPentrationTest",
                column: "TestsId");

            migrationBuilder.CreateIndex(
                name: "IX_PentrationTest_OwnerId",
                table: "PentrationTest",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PentrationTest_SystemId",
                table: "PentrationTest",
                column: "SystemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PentesterPentrationTest");

            migrationBuilder.DropTable(
                name: "PentestingMethodologyPentrationTest");

            migrationBuilder.DropTable(
                name: "PentestingMethodology");

            migrationBuilder.DropTable(
                name: "PentrationTest");

            migrationBuilder.DropTable(
                name: "TargetSystem");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Clients");
        }
    }
}
