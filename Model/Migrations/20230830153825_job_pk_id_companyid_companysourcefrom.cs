using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class job_pk_id_companyid_companysourcefrom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "job_pk",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.AlterColumn<string>(
                name: "company_source_from",
                schema: "jobseeker",
                table: "job",
                type: "character varying",
                nullable: false,
                defaultValueSql: "''::character varying",
                comment: "來源",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldComment: "來源");

            migrationBuilder.AddPrimaryKey(
                name: "job_pk",
                schema: "jobseeker",
                table: "job",
                columns: new[] { "id", "company_id", "company_source_from" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "job_pk",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.AlterColumn<string>(
                name: "company_source_from",
                schema: "jobseeker",
                table: "job",
                type: "character varying",
                nullable: false,
                comment: "來源",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldDefaultValueSql: "''::character varying",
                oldComment: "來源");

            migrationBuilder.AddPrimaryKey(
                name: "job_pk",
                schema: "jobseeker",
                table: "job",
                columns: new[] { "id", "company_id" });
        }
    }
}
