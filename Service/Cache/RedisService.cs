using Microsoft.Extensions.Logging;
using Service.Db;
using StackExchange.Redis;

namespace Service.Cache
{
    public abstract class RedisService : ICacheService
    {
        private readonly ILogger<RedisService> logger;
        private readonly IDatabase redisDb;
        private readonly IDbService dbService;

        public RedisService(ILogger<RedisService> logger,
            IDatabase redisDb,
            IDbService dbService)
        {
            this.logger = logger;
            this.redisDb = redisDb;
            this.dbService = dbService;
        }

        /// <summary>
        /// 公司存在
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<bool> CompanyExist(string redisKey, string companyId)
        {
            if (await redisDb.HashExistsAsync(redisKey, companyId) &&
                await redisDb.HashGetAsync(redisKey, companyId) == true)
                return true;

            if (!await dbService.CompanyExist(companyId))
                return false;

            await redisDb.HashSetAsync(redisKey, companyId, true);
            return true;
        }

        /// <summary>
        /// 職缺存在
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<bool> JobExist(string redisKey, string companyId, string jobId)
        {
            var field = companyId + ";" + jobId;
            if (await redisDb.HashExistsAsync(redisKey, field) &&
              await redisDb.HashGetAsync(redisKey, field) == true)
                return true;

            if (!await dbService.JobExist(companyId, jobId))
                return false;

            await redisDb.HashSetAsync(redisKey, field, true);
            return true;
        }

        public abstract Task ResetExistCompanyAndJob();
    }
}
