using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class SocialEntetiesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "UserScores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "TestComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentedAt",
                table: "TestComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "QuestionComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentedAt",
                table: "QuestionComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "ArticleComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentedAt",
                table: "ArticleComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "AnswerComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentedAt",
                table: "AnswerComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "UserScores");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "TestComments");

            migrationBuilder.DropColumn(
                name: "CommentedAt",
                table: "TestComments");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "QuestionComments");

            migrationBuilder.DropColumn(
                name: "CommentedAt",
                table: "QuestionComments");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "ArticleComments");

            migrationBuilder.DropColumn(
                name: "CommentedAt",
                table: "ArticleComments");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "AnswerComments");

            migrationBuilder.DropColumn(
                name: "CommentedAt",
                table: "AnswerComments");
        }
    }
}
