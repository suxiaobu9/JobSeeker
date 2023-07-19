using Model.Dto104;
using Service.Cache;
using StackExchange.Redis;

namespace Crawer_104.Service;

internal class Redis104Service : RedisService
{
    private readonly ILogger<RedisService> logger;
    private readonly IDatabase redisDb;

    public Redis104Service(ILogger<RedisService> logger, IDatabase redisDb) : base(logger, redisDb)
    {
        this.logger = logger;
        this.redisDb = redisDb;
    }

    public override async Task ResetExistCompanyAndJob()
    {
        logger.LogInformation($"{nameof(Redis104Service)} Reset exist company and job");
        await redisDb.KeyDeleteAsync(Parameters104.CompanyIdForRedisAndQueue);
        await redisDb.KeyDeleteAsync(Parameters104.JobIdForRedisAndQueue);
        await redisDb.KeyDeleteAsync(Parameters104.CompanyExistInDbRedisKey);
    }
}
