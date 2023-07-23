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

        /// <summary>
        /// key field 是否存在於 cache 中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<bool> IsKeyFieldExistsInCache(string key, string field)
        {
            var dataExist = await redisDb.HashExistsAsync(key, field);

            if (dataExist && await redisDb.HashGetAsync(key, field) == true)
                return true;

            var redisTrans = redisDb.CreateTransaction();

            // 1. 如果 key 不存在才執行下一個動作
            redisTrans.AddCondition(Condition.HashNotExists(key, field));

            // 2. 第 1 點條件成立才會執行
            var setValueTask = redisTrans.HashSetAsync(key, field, true);

            // 有完整執行到第 2 點 committed 才會是 true
            var committed = await redisTrans.ExecuteAsync();

            if (!committed && setValueTask.IsCanceled)
                return true;

            if (committed)
                return false;

            logger.LogWarning("{key} {field} committed {committed}. {IsCanceled},{IsCompleted},{IsCompletedSuccessfully},{IsFaulted}",
                key, field, committed, setValueTask.IsCanceled, setValueTask.IsCompleted, setValueTask.IsCompletedSuccessfully, setValueTask.IsFaulted);

            return true;
        }

        public abstract Task ResetExistCompanyAndJob();
    }
}
