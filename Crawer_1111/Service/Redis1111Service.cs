using Model.Dto1111;
using Service.Cache;
using Service.Db;
using StackExchange.Redis;

namespace Crawer_1111.Service;

internal class Redis1111Service : RedisService
{
    private readonly ILogger<RedisService> logger;
    private readonly IDatabase redisDb;
    private readonly IDbService dbService;

    public Redis1111Service(ILogger<RedisService> logger, IDatabase redisDb, IDbService dbService) : base(logger, redisDb, dbService)
    {
        this.logger = logger;
        this.redisDb = redisDb;
        this.dbService = dbService;
    }

    public override async Task ResetExistCompanyAndJob()
    {
        logger.LogInformation($"{nameof(Redis1111Service)} Reset exist company and job");
        await redisDb.KeyDeleteAsync(Parameters1111.RedisKeyForCompanyIdSendToQueue);
        await redisDb.KeyDeleteAsync(Parameters1111.RedisKeyForCompanyUpdated);
        await redisDb.KeyDeleteAsync(Parameters1111.RedisKeyForJobIdSendToQueue);
    }
}
