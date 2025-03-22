using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeScoringAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddReasonToResumeScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "ResumeScores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "ResumeScores");
        }
    }
}
