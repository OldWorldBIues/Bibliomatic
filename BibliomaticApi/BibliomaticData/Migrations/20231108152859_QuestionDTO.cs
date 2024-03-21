using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class QuestionDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionHtml",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "IndexOfImage",
                table: "ImageInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HyperlinkDescription",
                table: "HyperlinkInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IndexOfHyperlink",
                table: "HyperlinkInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IndexOfFormula",
                table: "FormulaInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndexOfImage",
                table: "ImageInfo");

            migrationBuilder.DropColumn(
                name: "HyperlinkDescription",
                table: "HyperlinkInfo");

            migrationBuilder.DropColumn(
                name: "IndexOfHyperlink",
                table: "HyperlinkInfo");

            migrationBuilder.DropColumn(
                name: "IndexOfFormula",
                table: "FormulaInfo");

            migrationBuilder.AddColumn<string>(
                name: "QuestionHtml",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
