using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeysTestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestAnswers_TestQuestions_Id",
                table: "TestAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestions_Tests_Id",
                table: "TestQuestions");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_TestId",
                table: "TestQuestions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_TestQuestionId",
                table: "TestAnswers",
                column: "TestQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestAnswers_TestQuestions_TestQuestionId",
                table: "TestAnswers",
                column: "TestQuestionId",
                principalTable: "TestQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestions_Tests_TestId",
                table: "TestQuestions",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestAnswers_TestQuestions_TestQuestionId",
                table: "TestAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestions_Tests_TestId",
                table: "TestQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TestQuestions_TestId",
                table: "TestQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TestAnswers_TestQuestionId",
                table: "TestAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_TestAnswers_TestQuestions_Id",
                table: "TestAnswers",
                column: "Id",
                principalTable: "TestQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestions_Tests_Id",
                table: "TestQuestions",
                column: "Id",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
