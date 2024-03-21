using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class QuestionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionHtml = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormulaInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormulaLatex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormulaInfo_Questions_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HyperlinkInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperlinkInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HyperlinkInfo_Questions_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageFilename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageInfo_Questions_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormulaInfo_OwnerId",
                table: "FormulaInfo",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_HyperlinkInfo_OwnerId",
                table: "HyperlinkInfo",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageInfo_OwnerId",
                table: "ImageInfo",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormulaInfo");

            migrationBuilder.DropTable(
                name: "HyperlinkInfo");

            migrationBuilder.DropTable(
                name: "ImageInfo");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
