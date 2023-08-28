using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class mult_company_job_pk_fk_index_20230828 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "job_fk",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.DropPrimaryKey(
                name: "PK_company",
                schema: "jobseeker",
                table: "company");

            migrationBuilder.DropIndex(
                name: "company_id_idx",
                schema: "jobseeker",
                table: "company");

            migrationBuilder.RenameIndex(
                name: "IX_job_company_id",
                schema: "jobseeker",
                table: "job",
                newName: "job_company_id_idx");

            migrationBuilder.AlterColumn<string>(
                name: "company_source_from",
                schema: "jobseeker",
                table: "job",
                type: "character varying",
                nullable: false,
                defaultValue: "",
                comment: "來源",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "company_pk",
                schema: "jobseeker",
                table: "company",
                columns: new[] { "id", "source_from" });

            migrationBuilder.CreateIndex(
                name: "IX_job_company_id_company_source_from",
                schema: "jobseeker",
                table: "job",
                columns: new[] { "company_id", "company_source_from" });

            migrationBuilder.AddForeignKey(
                name: "job_fk",
                schema: "jobseeker",
                table: "job",
                columns: new[] { "company_id", "company_source_from" },
                principalSchema: "jobseeker",
                principalTable: "company",
                principalColumns: new[] { "id", "source_from" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "job_fk",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.DropIndex(
                name: "IX_job_company_id_company_source_from",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.DropPrimaryKey(
                name: "company_pk",
                schema: "jobseeker",
                table: "company");

            migrationBuilder.RenameIndex(
                name: "job_company_id_idx",
                schema: "jobseeker",
                table: "job",
                newName: "IX_job_company_id");

            migrationBuilder.AlterColumn<string>(
                name: "company_source_from",
                schema: "jobseeker",
                table: "job",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldComment: "來源");

            migrationBuilder.AddPrimaryKey(
                name: "PK_company",
                schema: "jobseeker",
                table: "company",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "company_id_idx",
                schema: "jobseeker",
                table: "company",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "job_fk",
                schema: "jobseeker",
                table: "job",
                column: "company_id",
                principalSchema: "jobseeker",
                principalTable: "company",
                principalColumn: "id");
        }
    }
}
