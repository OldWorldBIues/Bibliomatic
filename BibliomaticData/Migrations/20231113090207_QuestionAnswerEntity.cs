using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class QuestionAnswerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseQuestions_Questions_QuestionId",
                table: "BaseQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_BaseQuestions_BaseQuestionId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "FormulaInfo");

            migrationBuilder.DropTable(
                name: "HyperlinkInfo");

            migrationBuilder.DropTable(
                name: "ImageInfo");

            migrationBuilder.DropIndex(
                name: "IX_Questions_BaseQuestionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_BaseQuestions_QuestionId",
                table: "BaseQuestions");

            migrationBuilder.DropColumn(
                name: "IsAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "BaseQuestions");

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAnswer = table.Column<bool>(type: "bit", nullable: false),
                    BaseQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_BaseQuestions_BaseQuestionId",
                        column: x => x.BaseQuestionId,
                        principalTable: "BaseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions_Formulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IndexOfFormula = table.Column<int>(type: "int", nullable: false),
                    FormulaLatex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaType = table.Column<int>(type: "int", nullable: false),
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
                    IndexOfHyperlink = table.Column<int>(type: "int", nullable: false),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    IndexOfImage = table.Column<int>(type: "int", nullable: false),
                    ImageFilename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Answers_Formulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IndexOfFormula = table.Column<int>(type: "int", nullable: false),
                    FormulaLatex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormulaType = table.Column<int>(type: "int", nullable: false),
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
                    IndexOfHyperlink = table.Column<int>(type: "int", nullable: false),
                    Hyperlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    IndexOfImage = table.Column<int>(type: "int", nullable: false),
                    ImageFilename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Questions_BaseQuestionId",
                table: "Questions",
                column: "BaseQuestionId",
                unique: true,
                filter: "[BaseQuestionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_BaseQuestionId",
                table: "Answers",
                column: "BaseQuestionId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_BaseQuestions_BaseQuestionId",
                table: "Questions",
                column: "BaseQuestionId",
                principalTable: "BaseQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_BaseQuestions_BaseQuestionId",
                table: "Questions");

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

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Questions_BaseQuestionId",
                table: "Questions");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnswer",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "BaseQuestions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormulaInfo",
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
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndexOfHyperlink = table.Column<int>(type: "int", nullable: false),
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
                    ImageType = table.Column<int>(type: "int", nullable: false),
                    IndexOfImage = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_Questions_BaseQuestionId",
                table: "Questions",
                column: "BaseQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseQuestions_QuestionId",
                table: "BaseQuestions",
                column: "QuestionId",
                unique: true,
                filter: "[QuestionId] IS NOT NULL");

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

            migrationBuilder.AddForeignKey(
                name: "FK_BaseQuestions_Questions_QuestionId",
                table: "BaseQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_BaseQuestions_BaseQuestionId",
                table: "Questions",
                column: "BaseQuestionId",
                principalTable: "BaseQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
