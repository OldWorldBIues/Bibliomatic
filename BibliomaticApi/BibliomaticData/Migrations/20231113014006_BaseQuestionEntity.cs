using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class BaseQuestionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "Questions");

            migrationBuilder.AddColumn<Guid>(
                name: "BaseQuestionId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsAnswer",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsQuestion",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ImageType",
                table: "ImageInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormulaType",
                table: "FormulaInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BaseQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSolved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseQuestions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_BaseQuestionId",
                table: "Questions",
                column: "BaseQuestionId");

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
                name: "BaseQuestions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_BaseQuestionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "BaseQuestionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsQuestion",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "ImageInfo");

            migrationBuilder.DropColumn(
                name: "FormulaType",
                table: "FormulaInfo");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
