using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class Infos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerFormulaInfo_Answers_AnswerId",
                table: "AnswerFormulaInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerHyperlinkInfo_Answers_AnswerId",
                table: "AnswerHyperlinkInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerImageInfo_Answers_AnswerId",
                table: "AnswerImageInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionFormulaInfo_Questions_QuestionId",
                table: "QuestionFormulaInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionHyperlinkInfo_Questions_QuestionId",
                table: "QuestionHyperlinkInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionImageInfo_Questions_QuestionId",
                table: "QuestionImageInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionImageInfo",
                table: "QuestionImageInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionHyperlinkInfo",
                table: "QuestionHyperlinkInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionFormulaInfo",
                table: "QuestionFormulaInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerImageInfo",
                table: "AnswerImageInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerHyperlinkInfo",
                table: "AnswerHyperlinkInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerFormulaInfo",
                table: "AnswerFormulaInfo");

            migrationBuilder.RenameTable(
                name: "QuestionImageInfo",
                newName: "QuestionImages");

            migrationBuilder.RenameTable(
                name: "QuestionHyperlinkInfo",
                newName: "QuestionHyperlinks");

            migrationBuilder.RenameTable(
                name: "QuestionFormulaInfo",
                newName: "QuestionFormula");

            migrationBuilder.RenameTable(
                name: "AnswerImageInfo",
                newName: "AnswerImages");

            migrationBuilder.RenameTable(
                name: "AnswerHyperlinkInfo",
                newName: "AnswerHyperlinks");

            migrationBuilder.RenameTable(
                name: "AnswerFormulaInfo",
                newName: "AnswerFormula");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionImageInfo_QuestionId",
                table: "QuestionImages",
                newName: "IX_QuestionImages_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionHyperlinkInfo_QuestionId",
                table: "QuestionHyperlinks",
                newName: "IX_QuestionHyperlinks_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionFormulaInfo_QuestionId",
                table: "QuestionFormula",
                newName: "IX_QuestionFormula_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerImageInfo_AnswerId",
                table: "AnswerImages",
                newName: "IX_AnswerImages_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerHyperlinkInfo_AnswerId",
                table: "AnswerHyperlinks",
                newName: "IX_AnswerHyperlinks_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerFormulaInfo_AnswerId",
                table: "AnswerFormula",
                newName: "IX_AnswerFormula_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionImages",
                table: "QuestionImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionHyperlinks",
                table: "QuestionHyperlinks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionFormula",
                table: "QuestionFormula",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerImages",
                table: "AnswerImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerHyperlinks",
                table: "AnswerHyperlinks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerFormula",
                table: "AnswerFormula",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerFormula_Answers_AnswerId",
                table: "AnswerFormula",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerHyperlinks_Answers_AnswerId",
                table: "AnswerHyperlinks",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerImages_Answers_AnswerId",
                table: "AnswerImages",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionFormula_Questions_QuestionId",
                table: "QuestionFormula",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionHyperlinks_Questions_QuestionId",
                table: "QuestionHyperlinks",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionImages_Questions_QuestionId",
                table: "QuestionImages",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerFormula_Answers_AnswerId",
                table: "AnswerFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerHyperlinks_Answers_AnswerId",
                table: "AnswerHyperlinks");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerImages_Answers_AnswerId",
                table: "AnswerImages");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionFormula_Questions_QuestionId",
                table: "QuestionFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionHyperlinks_Questions_QuestionId",
                table: "QuestionHyperlinks");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionImages_Questions_QuestionId",
                table: "QuestionImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionImages",
                table: "QuestionImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionHyperlinks",
                table: "QuestionHyperlinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionFormula",
                table: "QuestionFormula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerImages",
                table: "AnswerImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerHyperlinks",
                table: "AnswerHyperlinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerFormula",
                table: "AnswerFormula");

            migrationBuilder.RenameTable(
                name: "QuestionImages",
                newName: "QuestionImageInfo");

            migrationBuilder.RenameTable(
                name: "QuestionHyperlinks",
                newName: "QuestionHyperlinkInfo");

            migrationBuilder.RenameTable(
                name: "QuestionFormula",
                newName: "QuestionFormulaInfo");

            migrationBuilder.RenameTable(
                name: "AnswerImages",
                newName: "AnswerImageInfo");

            migrationBuilder.RenameTable(
                name: "AnswerHyperlinks",
                newName: "AnswerHyperlinkInfo");

            migrationBuilder.RenameTable(
                name: "AnswerFormula",
                newName: "AnswerFormulaInfo");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionImages_QuestionId",
                table: "QuestionImageInfo",
                newName: "IX_QuestionImageInfo_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionHyperlinks_QuestionId",
                table: "QuestionHyperlinkInfo",
                newName: "IX_QuestionHyperlinkInfo_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionFormula_QuestionId",
                table: "QuestionFormulaInfo",
                newName: "IX_QuestionFormulaInfo_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerImages_AnswerId",
                table: "AnswerImageInfo",
                newName: "IX_AnswerImageInfo_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerHyperlinks_AnswerId",
                table: "AnswerHyperlinkInfo",
                newName: "IX_AnswerHyperlinkInfo_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerFormula_AnswerId",
                table: "AnswerFormulaInfo",
                newName: "IX_AnswerFormulaInfo_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionImageInfo",
                table: "QuestionImageInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionHyperlinkInfo",
                table: "QuestionHyperlinkInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionFormulaInfo",
                table: "QuestionFormulaInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerImageInfo",
                table: "AnswerImageInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerHyperlinkInfo",
                table: "AnswerHyperlinkInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerFormulaInfo",
                table: "AnswerFormulaInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerFormulaInfo_Answers_AnswerId",
                table: "AnswerFormulaInfo",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerHyperlinkInfo_Answers_AnswerId",
                table: "AnswerHyperlinkInfo",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerImageInfo_Answers_AnswerId",
                table: "AnswerImageInfo",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionFormulaInfo_Questions_QuestionId",
                table: "QuestionFormulaInfo",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionHyperlinkInfo_Questions_QuestionId",
                table: "QuestionHyperlinkInfo",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionImageInfo_Questions_QuestionId",
                table: "QuestionImageInfo",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
