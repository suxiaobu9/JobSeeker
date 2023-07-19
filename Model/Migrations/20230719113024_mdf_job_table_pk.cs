using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class mdf_job_table_pk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_job",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.AddPrimaryKey(
                name: "job_pk",
                schema: "jobseeker",
                table: "job",
                columns: new[] { "id", "company_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "job_pk",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.AddPrimaryKey(
                name: "PK_job",
                schema: "jobseeker",
                table: "job",
                column: "id");
        }
    }
}
