using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class company_and_job_update_count_make_as_required : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "update_count",
                schema: "jobseeker",
                table: "job",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "手動更新次數",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValueSql: "0",
                oldComment: "手動更新次數");

            migrationBuilder.AlterColumn<int>(
                name: "update_count",
                schema: "jobseeker",
                table: "company",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "手動更新次數",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValueSql: "0",
                oldComment: "手動更新次數");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "update_count",
                schema: "jobseeker",
                table: "job",
                type: "integer",
                nullable: true,
                defaultValueSql: "0",
                comment: "手動更新次數",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "手動更新次數");

            migrationBuilder.AlterColumn<int>(
                name: "update_count",
                schema: "jobseeker",
                table: "company",
                type: "integer",
                nullable: true,
                defaultValueSql: "0",
                comment: "手動更新次數",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "手動更新次數");
        }
    }
}
