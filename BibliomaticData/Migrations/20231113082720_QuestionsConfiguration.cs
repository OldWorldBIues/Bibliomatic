using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class QuestionsConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_BaseQuestions_BaseQuestionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsQuestion",
                table: "Questions");

            migrationBuilder.AlterColumn<Guid>(
                name: "BaseQuestionId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "BaseQuestions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseQuestions_QuestionId",
                table: "BaseQuestions",
                column: "QuestionId",
                unique: true,
                filter: "[QuestionId] IS NOT NULL");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseQuestions_Questions_QuestionId",
                table: "BaseQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_BaseQuestions_BaseQuestionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_BaseQuestions_QuestionId",
                table: "BaseQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "BaseQuestions");

            migrationBuilder.AlterColumn<Guid>(
                name: "BaseQuestionId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQuestion",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_BaseQuestions_BaseQuestionId",
                table: "Questions",
                column: "BaseQuestionId",
                principalTable: "BaseQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
