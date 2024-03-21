using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliomaticData.Migrations
{
    /// <inheritdoc />
    public partial class TestEntitySocialReactionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestComment_Tests_TestId",
                table: "TestComment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserScore_Tests_TestId",
                table: "UserScore");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserScore",
                table: "UserScore");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestComment",
                table: "TestComment");

            migrationBuilder.RenameTable(
                name: "UserScore",
                newName: "UserScores");

            migrationBuilder.RenameTable(
                name: "TestComment",
                newName: "TestComments");

            migrationBuilder.RenameIndex(
                name: "IX_UserScore_TestId",
                table: "UserScores",
                newName: "IX_UserScores_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_TestComment_TestId",
                table: "TestComments",
                newName: "IX_TestComments_TestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserScores",
                table: "UserScores",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestComments",
                table: "TestComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestComments_Tests_TestId",
                table: "TestComments",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserScores_Tests_TestId",
                table: "UserScores",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestComments_Tests_TestId",
                table: "TestComments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserScores_Tests_TestId",
                table: "UserScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserScores",
                table: "UserScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestComments",
                table: "TestComments");

            migrationBuilder.RenameTable(
                name: "UserScores",
                newName: "UserScore");

            migrationBuilder.RenameTable(
                name: "TestComments",
                newName: "TestComment");

            migrationBuilder.RenameIndex(
                name: "IX_UserScores_TestId",
                table: "UserScore",
                newName: "IX_UserScore_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_TestComments_TestId",
                table: "TestComment",
                newName: "IX_TestComment_TestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserScore",
                table: "UserScore",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestComment",
                table: "TestComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestComment_Tests_TestId",
                table: "TestComment",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserScore_Tests_TestId",
                table: "UserScore",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
