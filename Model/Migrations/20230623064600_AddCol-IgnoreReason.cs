using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class AddColIgnoreReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ignore_reason",
                schema: "jobseeker",
                table: "job",
                type: "text",
                nullable: true,
                comment: "忽略不看");

            migrationBuilder.AddColumn<string>(
                name: "ignore_reason",
                schema: "jobseeker",
                table: "company",
                type: "text",
                nullable: true,
                comment: "忽略理由");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ignore_reason",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.DropColumn(
                name: "ignore_reason",
                schema: "jobseeker",
                table: "company");
        }
    }
}
