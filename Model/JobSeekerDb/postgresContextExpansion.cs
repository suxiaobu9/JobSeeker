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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=postgres;User Id=jobseeker;Password=jobseeker");
            }

        }
    }
}
