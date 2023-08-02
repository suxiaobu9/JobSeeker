using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class rm_job_company_id_and_job_id_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "job_id_idx",
                schema: "jobseeker",
                table: "job");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "job_id_idx",
                schema: "jobseeker",
                table: "job",
                column: "id",
                unique: true);
        }
    }
}
