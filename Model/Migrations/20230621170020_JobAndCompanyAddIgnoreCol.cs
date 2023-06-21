using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class JobAndCompanyAddIgnoreCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ignore",
                schema: "jobseeker",
                table: "job",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "忽略不看",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValueSql: "false",
                oldComment: "忽略不看");

            migrationBuilder.AlterColumn<bool>(
                name: "ignore",
                schema: "jobseeker",
                table: "company",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "忽略不看",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValueSql: "false",
                oldComment: "忽略不看");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ignore",
                schema: "jobseeker",
                table: "job",
                type: "boolean",
                nullable: true,
                defaultValueSql: "false",
                comment: "忽略不看",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "忽略不看");

            migrationBuilder.AlterColumn<bool>(
                name: "ignore",
                schema: "jobseeker",
                table: "company",
                type: "boolean",
                nullable: true,
                defaultValueSql: "false",
                comment: "忽略不看",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "忽略不看");
        }
    }
}
