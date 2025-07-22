using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserQuizResults_UserQuizResultId1",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_UserQuizResultId1",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "UserQuizResultId1",
                table: "UserAnswers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserQuizResultId1",
                table: "UserAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserQuizResultId1",
                table: "UserAnswers",
                column: "UserQuizResultId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_UserQuizResults_UserQuizResultId1",
                table: "UserAnswers",
                column: "UserQuizResultId1",
                principalTable: "UserQuizResults",
                principalColumn: "Id");
        }
    }
}
