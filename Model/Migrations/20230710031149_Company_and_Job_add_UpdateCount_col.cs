using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class Company_and_Job_add_UpdateCount_col : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "get_info_url",
                schema: "jobseeker",
                table: "job",
                type: "text",
                nullable: false,
                defaultValueSql: "''::text",
                comment: "取得資訊的網址",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "取得資訊的網址");

            migrationBuilder.AddColumn<int>(
                name: "update_count",
                schema: "jobseeker",
                table: "job",
                type: "integer",
                nullable: true,
                defaultValueSql: "0",
                comment: "手動更新次數");

            migrationBuilder.AlterColumn<string>(
                name: "get_info_url",
                schema: "jobseeker",
                table: "company",
                type: "text",
                nullable: false,
                defaultValueSql: "''::text",
                comment: "取得資料的網址",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "取得資料的網址");

            migrationBuilder.AddColumn<int>(
                name: "update_count",
                schema: "jobseeker",
                table: "company",
                type: "integer",
                nullable: true,
                defaultValueSql: "0",
                comment: "手動更新次數");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "update_count",
                schema: "jobseeker",
                table: "job");

            migrationBuilder.DropColumn(
                name: "update_count",
                schema: "jobseeker",
                table: "company");

            migrationBuilder.AlterColumn<string>(
                name: "get_info_url",
                schema: "jobseeker",
                table: "job",
                type: "text",
                nullable: false,
                comment: "取得資訊的網址",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "''::text",
                oldComment: "取得資訊的網址");

            migrationBuilder.AlterColumn<string>(
                name: "get_info_url",
                schema: "jobseeker",
                table: "company",
                type: "text",
                nullable: false,
                comment: "取得資料的網址",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "''::text",
                oldComment: "取得資料的網址");
        }
    }
}
