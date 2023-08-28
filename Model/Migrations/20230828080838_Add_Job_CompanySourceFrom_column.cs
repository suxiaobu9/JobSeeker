using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class Add_Job_CompanySourceFrom_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "company_source_from",
                schema: "jobseeker",
                table: "job",
                type: "character varying",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "source_from",
                schema: "jobseeker",
                table: "company",
                type: "character varying",
                nullable: false,
                comment: "來源",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldComment: "來源");

            migrationBuilder.Sql("UPDATE job SET company_source_from = company.source_from FROM company WHERE job.company_id = company.id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "company_source_from",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.AlterColumn<string>(
                name: "source_from",
                schema: "jobseeker",
                table: "company",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                comment: "來源",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldComment: "來源");

        }
    }
}
