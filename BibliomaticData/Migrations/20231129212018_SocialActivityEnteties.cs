using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class SocialActivityEnteties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TestQuestionFormulaFilename",
                table: "TestQuestions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestAnswerFormulaFilename",
                table: "TestAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestVariantFormulaFilename",
                table: "TestAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TestQuestionFormulaFilename",
                table: "TestQuestions");

            migrationBuilder.DropColumn(
                name: "TestAnswerFormulaFilename",
                table: "TestAnswers");

            migrationBuilder.DropColumn(
                name: "TestVariantFormulaFilename",
                table: "TestAnswers");

            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Answers");
        }
    }
}
