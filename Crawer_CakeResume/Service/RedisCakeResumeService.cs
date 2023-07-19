using Model.Dto104;
using Model.DtoCakeResume;
using Service.Cache;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer_CakeResume.Service;

public class RedisCakeResumeService : RedisService
{
    private readonly ILogger<RedisService> logger;
    private readonly IDatabase redisDb;

    public RedisCakeResumeService(ILogger<RedisService> logger, IDatabase redisDb) : base(logger, redisDb)
    {
        this.logger = logger;
        this.redisDb = redisDb;
    }

    public override async Task ResetExistCompanyAndJob()
    {
        logger.LogInformation($"{nameof(RedisCakeResumeService)} Reset exist company and job");
        await redisDb.KeyDeleteAsync(ParametersCakeResume.CompanyIdForRedisAndQueue);
        await redisDb.KeyDeleteAsync(ParametersCakeResume.JobIdForRedisAndQueue);
    }
}
