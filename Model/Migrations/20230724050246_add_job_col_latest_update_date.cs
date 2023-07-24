using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class add_job_col_latest_update_date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "latest_update_date",
                schema: "jobseeker",
                table: "job",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "最後更新時間");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latest_update_date",
                schema: "jobseeker",
                table: "job");
        }
    }
}
