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
        }
    }
}
