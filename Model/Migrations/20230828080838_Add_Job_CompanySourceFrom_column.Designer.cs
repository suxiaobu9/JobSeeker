﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.JobSeekerDb;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Model.Migrations
{
    [DbContext(typeof(postgresContext))]
    [Migration("20230828080838_Add_Job_CompanySourceFrom_column")]
    partial class Add_Job_CompanySourceFrom_column
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Model.JobSeekerDb.Company", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreateUtcAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_utc_at")
                        .HasComment("建立時間");

                    b.Property<string>("GetInfoUrl")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("get_info_url")
                        .HasDefaultValueSql("''::text")
                        .HasComment("取得資料的網址");

                    b.Property<bool>("Ignore")
                        .HasColumnType("boolean")
                        .HasColumnName("ignore")
                        .HasComment("忽略不看");

                    b.Property<string>("IgnoreReason")
                        .HasColumnType("text")
                        .HasColumnName("ignore_reason")
                        .HasComment("忽略理由");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name")
                        .HasComment("公司名稱");

                    b.Property<string>("Product")
                        .HasColumnType("text")
                        .HasColumnName("product")
                        .HasComment("主要商品/服務");

                    b.Property<string>("Profile")
                        .HasColumnType("text")
                        .HasColumnName("profile")
                        .HasComment("公司描述");

                    b.Property<int>("Sort")
                        .HasColumnType("integer")
                        .HasColumnName("sort")
                        .HasComment("排序");

                    b.Property<string>("SourceFrom")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("source_from")
                        .HasComment("來源");

                    b.Property<int>("UpdateCount")
                        .HasColumnType("integer")
                        .HasColumnName("update_count")
                        .HasComment("手動更新次數");

                    b.Property<DateTime?>("UpdateUtcAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("update_utc_at")
                        .HasComment("更新時間");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url")
                        .HasComment("公司網址");

                    b.Property<string>("Welfare")
                        .HasColumnType("text")
                        .HasColumnName("welfare")
                        .HasComment("福利");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "company_id_idx")
                        .IsUnique();

                    b.ToTable("company", "jobseeker");
                });

            modelBuilder.Entity("Model.JobSeekerDb.Job", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying")
                        .HasColumnName("id");

                    b.Property<string>("CompanyId")
                        .HasColumnType("character varying")
                        .HasColumnName("company_id");

                    b.Property<string>("CompanySourceFrom")
                        .HasColumnType("character varying")
                        .HasColumnName("company_source_from");

                    b.Property<DateTime>("CreateUtcAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_utc_at")
                        .HasComment("建立時間");

                    b.Property<string>("GetInfoUrl")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("get_info_url")
                        .HasDefaultValueSql("''::text")
                        .HasComment("取得資訊的網址");

                    b.Property<bool>("HaveRead")
                        .HasColumnType("boolean")
                        .HasColumnName("have_read")
                        .HasComment("已讀");

                    b.Property<bool>("Ignore")
                        .HasColumnType("boolean")
                        .HasColumnName("ignore")
                        .HasComment("忽略不看");

                    b.Property<string>("IgnoreReason")
                        .HasColumnType("text")
                        .HasColumnName("ignore_reason")
                        .HasComment("忽略不看");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted")
                        .HasComment("已刪除");

                    b.Property<string>("JobPlace")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("job_place")
                        .HasComment("工作地點");

                    b.Property<string>("LatestUpdateDate")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("latest_update_date")
                        .HasComment("最後更新時間");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("name")
                        .HasComment("名稱");

                    b.Property<string>("OtherRequirement")
                        .HasColumnType("text")
                        .HasColumnName("other_requirement")
                        .HasComment("其他要求");

                    b.Property<string>("Salary")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("salary")
                        .HasComment("薪資");

                    b.Property<int>("Sort")
                        .HasColumnType("integer")
                        .HasColumnName("sort")
                        .HasComment("排序");

                    b.Property<int>("UpdateCount")
                        .HasColumnType("integer")
                        .HasColumnName("update_count")
                        .HasComment("手動更新次數");

                    b.Property<DateTime?>("UpdateUtcAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("update_utc_at")
                        .HasComment("更新時間");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url")
                        .HasComment("職缺網址");

                    b.Property<string>("WorkContent")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("work_content")
                        .HasComment("工作內容");

                    b.HasKey("Id", "CompanyId")
                        .HasName("job_pk");

                    b.HasIndex(new[] { "CompanyId" }, "IX_job_company_id");

                    b.ToTable("job", "jobseeker");
                });

            modelBuilder.Entity("Model.JobSeekerDb.Job", b =>
                {
                    b.HasOne("Model.JobSeekerDb.Company", "Company")
                        .WithMany("Jobs")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("job_fk");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Model.JobSeekerDb.Company", b =>
                {
                    b.Navigation("Jobs");
                });
#pragma warning restore 612, 618
        }
    }
}
