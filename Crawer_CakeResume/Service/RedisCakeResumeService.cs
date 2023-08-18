using Model.DtoCakeResume;
using Service.Cache;
using Service.Db;
using StackExchange.Redis;

namespace Crawer_CakeResume.Service;

public class RedisCakeResumeService : RedisService
{
    private readonly ILogger<RedisService> logger;
    private readonly IDatabase redisDb;
    private readonly IDbService dbService;

    public RedisCakeResumeService(ILogger<RedisService> logger, IDatabase redisDb, IDbService dbService) : base(logger, redisDb, dbService)
    {
        this.logger = logger;
        this.redisDb = redisDb;
        this.dbService = dbService;
    }

    public override async Task ResetExistCompanyAndJob()
    {
        logger.LogInformation($"{nameof(RedisCakeResumeService)} Reset exist company and job");
        await redisDb.KeyDeleteAsync(ParametersCakeResume.RedisKeyForCompanyUpdated);

    }
}
