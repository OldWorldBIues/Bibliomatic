using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class AttachmentInfosConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormulaInfo");

            migrationBuilder.DropTable(
                name: "HyperlinkInfo");

            migrationBuilder.DropTable(
                name: "ImageInfo");

            migrationBuilder.CreateTable(
                name: "AnswerFormulaInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FormulaLatex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaFilename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerFormulaInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerFormulaInfo_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnswerHyperlinkInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerHyperlinkInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerHyperlinkInfo_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnswerImageInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageFilename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerImageInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerImageInfo_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionFormulaInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FormulaLatex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaFilename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionFormulaInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionFormulaInfo_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionHyperlinkInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionHyperlinkInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionHyperlinkInfo_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionImageInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageFilename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionImageInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionImageInfo_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerFormulaInfo_AnswerId",
                table: "AnswerFormulaInfo",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerHyperlinkInfo_AnswerId",
                table: "AnswerHyperlinkInfo",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerImageInfo_AnswerId",
                table: "AnswerImageInfo",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionFormulaInfo_QuestionId",
                table: "QuestionFormulaInfo",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionHyperlinkInfo_QuestionId",
                table: "QuestionHyperlinkInfo",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionImageInfo_QuestionId",
                table: "QuestionImageInfo",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerFormulaInfo");

            migrationBuilder.DropTable(
                name: "AnswerHyperlinkInfo");

            migrationBuilder.DropTable(
                name: "AnswerImageInfo");

            migrationBuilder.DropTable(
                name: "QuestionFormulaInfo");

            migrationBuilder.DropTable(
                name: "QuestionHyperlinkInfo");

            migrationBuilder.DropTable(
                name: "QuestionImageInfo");

            migrationBuilder.CreateTable(
                name: "FormulaInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FormulaFilename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaLatex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageFilename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
    }
}
