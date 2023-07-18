using Azure.Messaging.ServiceBus;
using Model.Dto;
using Model.Dto104;
using Service.Cache;
using Service.Db;
using Service.Http;
using Service.Mq;

namespace Crawer_104.Workers
{
    public class CompanyToDbWorker : BackgroundService
    {
        private readonly ILogger<CompanyToDbWorker> logger;
        private readonly IMqService mqService;
        private readonly ICacheService cacheService;
        private readonly IHttpService httpService;
        private readonly IDbService dbService;

        public CompanyToDbWorker(ILogger<CompanyToDbWorker> logger,
            IMqService mqService,
            ICacheService cacheService,
            IHttpService httpService,
            IDbService dbService)
        {
            this.logger = logger;
            this.mqService = mqService;
            this.cacheService = cacheService;
            this.httpService = httpService;
            this.dbService = dbService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"{nameof(CompanyToDbWorker)} ExecuteAsync start.");
            await mqService.ProcessMessageFromMq(Parameters104.CompanyIdForRedisAndQueue, MessageHandler);

            while (true)
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        /// <summary>
        /// 處理 Company id mq 訊息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            // get mq (comp_id_for_104)
            string companyId = args.Message.Body.ToString();

            try
            {
                // get company dto
                var companyDto = await httpService.GetCompanyInfo<CompanyDto>(companyId, Parameters104.Get104CompanyInfoUrl(companyId));

                if (companyDto == null)
                {
                    logger.LogWarning($"{nameof(CompanyToDbWorker)} MessageHandler get null companyDto.{{companyId}}", companyId);
                    await args.CompleteMessageAsync(args.Message);
                    return;
                }

                // save to db
                await dbService.UpsertCompany(companyDto);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(CompanyToDbWorker)} MessageHandler get exception.{{companyId}}", companyId);
            }
            await args.CompleteMessageAsync(args.Message);
        }


    }
}
