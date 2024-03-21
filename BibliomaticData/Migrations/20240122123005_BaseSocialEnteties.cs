using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class BaseSocialEnteties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentedAt",
                table: "TestComments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CommentedAt",
                table: "QuestionComments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CommentedAt",
                table: "ArticleComments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CommentedAt",
                table: "AnswerComments",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "TestLikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "TestLikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "TestDislikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "TestDislikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "TestComments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "QuestionLikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "QuestionLikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "QuestionDislikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "QuestionDislikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "QuestionComments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ArticleLikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "ArticleLikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ArticleDislikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "ArticleDislikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ArticleComments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AnswerLikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "AnswerLikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AnswerDislikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "AnswerDislikes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AnswerComments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TestLikes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TestLikes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TestDislikes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TestDislikes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TestComments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "QuestionLikes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "QuestionLikes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "QuestionDislikes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "QuestionDislikes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "QuestionComments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ArticleLikes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ArticleLikes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ArticleDislikes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ArticleDislikes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ArticleComments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AnswerLikes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AnswerLikes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AnswerDislikes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AnswerDislikes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AnswerComments");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "TestComments",
                newName: "CommentedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "QuestionComments",
                newName: "CommentedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ArticleComments",
                newName: "CommentedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "AnswerComments",
                newName: "CommentedAt");
        }
    }
}
