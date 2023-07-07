using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class company_add_getinfourl_modify_to_not_null : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "get_info_url",
                schema: "jobseeker",
                table: "company",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "取得資料的網址",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValueSql: "''::text",
                oldComment: "取得資料的網址");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "get_info_url",
                schema: "jobseeker",
                table: "company",
                type: "text",
                nullable: true,
                defaultValueSql: "''::text",
                comment: "取得資料的網址",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "取得資料的網址");
        }
    }
}
