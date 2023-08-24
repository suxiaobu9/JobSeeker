using Model.Dto104;
using Model.DtoYourator;
using Service.Cache;
using Service.Db;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer_Yourator.Service;

public class RedisYouratorService : RedisService
{
    private readonly ILogger<RedisService> logger;
    private readonly IDatabase redisDb;
    private readonly IDbService dbService;

    public RedisYouratorService(ILogger<RedisService> logger, IDatabase redisDb, IDbService dbService) : base(logger, redisDb, dbService)
    {
        this.logger = logger;
        this.redisDb = redisDb;
        this.dbService = dbService;
    }

    public async override Task ResetExistCompanyAndJob()
    {
        logger.LogInformation($"{nameof(RedisYouratorService)} Reset exist company and job");
        await redisDb.KeyDeleteAsync(ParametersYourator.RedisKeyForCompanyIdSendToQueue);
        await redisDb.KeyDeleteAsync(ParametersYourator.RedisKeyForJobIdSendToQueue);
    }
}
