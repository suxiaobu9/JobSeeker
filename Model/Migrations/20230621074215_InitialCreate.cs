using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "jobseeker");

            migrationBuilder.CreateTable(
                name: "company",
                schema: "jobseeker",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "公司名稱"),
                    url = table.Column<string>(type: "text", nullable: false, comment: "公司網址"),
                    create_utc_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "建立時間"),
                    update_utc_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "更新時間"),
                    sort = table.Column<int>(type: "integer", nullable: false, comment: "排序")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job",
                schema: "jobseeker",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false, comment: "名稱"),
                    work_content = table.Column<string>(type: "text", nullable: false, comment: "工作內容"),
                    url = table.Column<string>(type: "text", nullable: false, comment: "職缺網址"),
                    salary = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "薪資"),
                    other_requirement = table.Column<string>(type: "text", nullable: true, comment: "其他要求"),
                    have_read = table.Column<bool>(type: "boolean", nullable: false, comment: "已讀"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, comment: "已刪除"),
                    job_place = table.Column<string>(type: "character varying", nullable: false, comment: "工作地點"),
                    company_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    create_utc_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "建立時間"),
                    update_utc_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "更新時間"),
                    sort = table.Column<int>(type: "integer", nullable: false, comment: "排序")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job", x => x.id);
                    table.ForeignKey(
                        name: "job_fk",
                        column: x => x.company_id,
                        principalSchema: "jobseeker",
                        principalTable: "company",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "company_id_idx",
                schema: "jobseeker",
                table: "company",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_company_id",
                schema: "jobseeker",
                table: "job",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "job_id_idx",
                schema: "jobseeker",
                table: "job",
                column: "id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job",
                schema: "jobseeker");

            migrationBuilder.DropTable(
                name: "company",
                schema: "jobseeker");
        }
    }
}
