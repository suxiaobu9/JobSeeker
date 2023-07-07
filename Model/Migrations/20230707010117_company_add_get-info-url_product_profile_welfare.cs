using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class company_add_getinfourl_product_profile_welfare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "get_info_url",
                schema: "jobseeker",
                table: "company",
                type: "text",
                nullable: true,
                defaultValueSql: "''::text",
                comment: "取得資料的網址");

            migrationBuilder.AddColumn<string>(
                name: "product",
                schema: "jobseeker",
                table: "company",
                type: "text",
                nullable: true,
                comment: "主要商品/服務");

            migrationBuilder.AddColumn<string>(
                name: "profile",
                schema: "jobseeker",
                table: "company",
                type: "text",
                nullable: true,
                comment: "公司描述");

            migrationBuilder.AddColumn<string>(
                name: "welfare",
                schema: "jobseeker",
                table: "company",
                type: "text",
                nullable: true,
                comment: "福利");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "get_info_url",
                schema: "jobseeker",
                table: "company");

            migrationBuilder.DropColumn(
                name: "product",
                schema: "jobseeker",
                table: "company");

            migrationBuilder.DropColumn(
                name: "profile",
                schema: "jobseeker",
                table: "company");

            migrationBuilder.DropColumn(
                name: "welfare",
                schema: "jobseeker",
                table: "company");
        }
    }
}
