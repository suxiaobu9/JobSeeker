using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Model.JobSeekerDb
{
    public partial class postgresContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 不紀錄 Log
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => { }));

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=postgres;User Id=jobseeker;Password=jobseeker");
            }

        }
    }
}
