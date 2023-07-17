using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class company_add_source_from_col : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "company_id",
                schema: "jobseeker",
                table: "job",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "jobseeker",
                table: "job",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "jobseeker",
                table: "company",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "source_from",
                schema: "jobseeker",
                table: "company",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValueSql: "'104'::character varying",
                comment: "來源");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "source_from",
                schema: "jobseeker",
                table: "company");

            migrationBuilder.AlterColumn<string>(
                name: "company_id",
                schema: "jobseeker",
                table: "job",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "jobseeker",
                table: "job",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "jobseeker",
                table: "company",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying");
        }
    }
}
