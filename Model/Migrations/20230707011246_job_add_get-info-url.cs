using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class job_add_getinfourl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "get_info_url",
                schema: "jobseeker",
                table: "job",
                type: "text",
                nullable: true,
                defaultValueSql: "''::text",
                comment: "取得資訊的網址");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "get_info_url",
                schema: "jobseeker",
                table: "job");
        }
    }
}
