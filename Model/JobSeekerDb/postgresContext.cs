using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Model.JobSeekerDb
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("company", "jobseeker");

                entity.HasIndex(e => e.Id, "company_id_idx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("character varying")
                    .HasColumnName("id");

                entity.Property(e => e.CreateUtcAt)
                    .HasColumnName("create_utc_at")
                    .HasComment("建立時間");

                entity.Property(e => e.GetInfoUrl)
                    .HasColumnName("get_info_url")
                    .HasDefaultValueSql("''::text")
                    .HasComment("取得資料的網址");

                entity.Property(e => e.Ignore)
                    .HasColumnName("ignore")
                    .HasComment("忽略不看");

                entity.Property(e => e.IgnoreReason)
                    .HasColumnName("ignore_reason")
                    .HasComment("忽略理由");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name")
                    .HasComment("公司名稱");

                entity.Property(e => e.Product)
                    .HasColumnName("product")
                    .HasComment("主要商品/服務");

                entity.Property(e => e.Profile)
                    .HasColumnName("profile")
                    .HasComment("公司描述");

                entity.Property(e => e.Sort)
                    .HasColumnName("sort")
                    .HasComment("排序");

                entity.Property(e => e.SourceFrom)
                    .HasMaxLength(20)
                    .HasColumnName("source_from")
                    .HasComment("來源");

                entity.Property(e => e.UpdateCount)
                    .HasColumnName("update_count")
                    .HasComment("手動更新次數");

                entity.Property(e => e.UpdateUtcAt)
                    .HasColumnName("update_utc_at")
                    .HasComment("更新時間");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasComment("公司網址");

                entity.Property(e => e.Welfare)
                    .HasColumnName("welfare")
                    .HasComment("福利");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("job", "jobseeker");

                entity.HasIndex(e => e.CompanyId, "IX_job_company_id");

                entity.HasIndex(e => e.Id, "job_id_idx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("character varying")
                    .HasColumnName("id");

                entity.Property(e => e.CompanyId)
                    .HasColumnType("character varying")
                    .HasColumnName("company_id");

                entity.Property(e => e.CreateUtcAt)
                    .HasColumnName("create_utc_at")
                    .HasComment("建立時間");

                entity.Property(e => e.GetInfoUrl)
                    .HasColumnName("get_info_url")
                    .HasDefaultValueSql("''::text")
                    .HasComment("取得資訊的網址");

                entity.Property(e => e.HaveRead)
                    .HasColumnName("have_read")
                    .HasComment("已讀");

                entity.Property(e => e.Ignore)
                    .HasColumnName("ignore")
                    .HasComment("忽略不看");

                entity.Property(e => e.IgnoreReason)
                    .HasColumnName("ignore_reason")
                    .HasComment("忽略不看");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasComment("已刪除");

                entity.Property(e => e.JobPlace)
                    .HasColumnType("character varying")
                    .HasColumnName("job_place")
                    .HasComment("工作地點");

                entity.Property(e => e.Name)
                    .HasMaxLength(300)
                    .HasColumnName("name")
                    .HasComment("名稱");

                entity.Property(e => e.OtherRequirement)
                    .HasColumnName("other_requirement")
                    .HasComment("其他要求");

                entity.Property(e => e.Salary)
                    .HasMaxLength(50)
                    .HasColumnName("salary")
                    .HasComment("薪資");

                entity.Property(e => e.Sort)
                    .HasColumnName("sort")
                    .HasComment("排序");

                entity.Property(e => e.UpdateCount)
                    .HasColumnName("update_count")
                    .HasComment("手動更新次數");

                entity.Property(e => e.UpdateUtcAt)
                    .HasColumnName("update_utc_at")
                    .HasComment("更新時間");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasComment("職缺網址");

                entity.Property(e => e.WorkContent)
                    .HasColumnName("work_content")
                    .HasComment("工作內容");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("job_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
