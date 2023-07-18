using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class rm_source_from_default_value : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "source_from",
                schema: "jobseeker",
                table: "company",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                comment: "來源",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValueSql: "'104'::character varying",
                oldComment: "來源");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "source_from",
                schema: "jobseeker",
                table: "company",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValueSql: "'104'::character varying",
                comment: "來源",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldComment: "來源");
        }
    }
}
