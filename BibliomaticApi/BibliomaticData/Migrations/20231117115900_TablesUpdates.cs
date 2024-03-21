using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class TablesUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerFormula_Answers_AnswerId",
                table: "AnswerFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionFormula_Questions_QuestionId",
                table: "QuestionFormula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionFormula",
                table: "QuestionFormula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerFormula",
                table: "AnswerFormula");

            migrationBuilder.RenameTable(
                name: "QuestionFormula",
                newName: "QuestionFormulas");

            migrationBuilder.RenameTable(
                name: "AnswerFormula",
                newName: "AnswerFormulas");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionFormula_QuestionId",
                table: "QuestionFormulas",
                newName: "IX_QuestionFormulas_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerFormula_AnswerId",
                table: "AnswerFormulas",
                newName: "IX_AnswerFormulas_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionFormulas",
                table: "QuestionFormulas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerFormulas",
                table: "AnswerFormulas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerFormulas_Answers_AnswerId",
                table: "AnswerFormulas",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionFormulas_Questions_QuestionId",
                table: "QuestionFormulas",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerFormulas_Answers_AnswerId",
                table: "AnswerFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionFormulas_Questions_QuestionId",
                table: "QuestionFormulas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionFormulas",
                table: "QuestionFormulas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerFormulas",
                table: "AnswerFormulas");

            migrationBuilder.RenameTable(
                name: "QuestionFormulas",
                newName: "QuestionFormula");

            migrationBuilder.RenameTable(
                name: "AnswerFormulas",
                newName: "AnswerFormula");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionFormulas_QuestionId",
                table: "QuestionFormula",
                newName: "IX_QuestionFormula_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerFormulas_AnswerId",
                table: "AnswerFormula",
                newName: "IX_AnswerFormula_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionFormula",
                table: "QuestionFormula",
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
                name: "FK_QuestionFormula_Questions_QuestionId",
                table: "QuestionFormula",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
