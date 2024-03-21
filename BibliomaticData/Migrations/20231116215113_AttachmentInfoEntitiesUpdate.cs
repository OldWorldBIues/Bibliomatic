using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class AttachmentInfoEntitiesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers_Formulas");

            migrationBuilder.DropTable(
                name: "Answers_Hyperlinks");

            migrationBuilder.DropTable(
                name: "Answers_Images");

            migrationBuilder.DropTable(
                name: "Questions_Formulas");

            migrationBuilder.DropTable(
                name: "Questions_Hyperlinks");

            migrationBuilder.DropTable(
                name: "Questions_Images");

            migrationBuilder.CreateTable(
                name: "FormulaInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FormulaLatex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaFilename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormulaInfo_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormulaInfo_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HyperlinkInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperlinkInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HyperlinkInfo_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HyperlinkInfo_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImageInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageFilename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageInfo_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImageInfo_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormulaInfo_AnswerId",
                table: "FormulaInfo",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaInfo_QuestionId",
                table: "FormulaInfo",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_HyperlinkInfo_AnswerId",
                table: "HyperlinkInfo",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_HyperlinkInfo_QuestionId",
                table: "HyperlinkInfo",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageInfo_AnswerId",
                table: "ImageInfo",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageInfo_QuestionId",
                table: "ImageInfo",
                column: "QuestionId");
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

            migrationBuilder.CreateTable(
                name: "Answers_Formulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormulaFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaLatex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaType = table.Column<int>(type: "int", nullable: false),
                    IndexOfFormula = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers_Formulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Formulas_Answers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers_Hyperlinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndexOfHyperlink = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers_Hyperlinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Hyperlinks_Answers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers_Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageFilename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<int>(type: "int", nullable: false),
                    IndexOfImage = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Images_Answers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions_Formulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormulaFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaLatex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaType = table.Column<int>(type: "int", nullable: false),
                    IndexOfFormula = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions_Formulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Formulas_Questions_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions_Hyperlinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndexOfHyperlink = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions_Hyperlinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Hyperlinks_Questions_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions_Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageFilename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<int>(type: "int", nullable: false),
                    IndexOfImage = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Images_Questions_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Formulas_OwnerId",
                table: "Answers_Formulas",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Hyperlinks_OwnerId",
                table: "Answers_Hyperlinks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Images_OwnerId",
                table: "Answers_Images",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Formulas_OwnerId",
                table: "Questions_Formulas",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Hyperlinks_OwnerId",
                table: "Questions_Hyperlinks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Images_OwnerId",
                table: "Questions_Images",
                column: "OwnerId");
        }
    }
}
