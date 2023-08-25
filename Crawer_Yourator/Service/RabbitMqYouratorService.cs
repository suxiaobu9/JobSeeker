using Model;
using Model.Dto;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Data;
using Service.Mq;
using System.Text;
using System.Text.Json;

namespace Crawer_Yourator.Service
{
    public class RabbitMqYouratorService : RabbitMqService
    {
        private readonly ILogger<RabbitMqService> logger;
        private readonly IDataService dataService;
        private readonly IConnection connection;

        public RabbitMqYouratorService(ILogger<RabbitMqService> logger,
            IDataService dataService,
            IConnection connection) : base(logger, connection)
        {
            this.logger = logger;
            this.dataService = dataService;
            this.connection = connection;
        }

        public async override Task<ReturnStatus> CompanyMessageHandler<T>(T args)
        {
            logger.LogInformation($"{nameof(RabbitMqYouratorService)} CompanyMessageHandler start.");

            if (args is not BasicDeliverEventArgs basicDeliverEventArgs)
            {
                logger.LogError($"{nameof(RabbitMqYouratorService)} CompanyMessageHandler basicDeliverEventArgs is null.");
                return ReturnStatus.Fail;
            }

            try
            {
                // get mq (comp_id_for_104)
                var body = basicDeliverEventArgs.Body.ToArray();
                var companyId = Encoding.UTF8.GetString(body);

                if (string.IsNullOrWhiteSpace(companyId))
                {
                    logger.LogError($"{nameof(RabbitMqYouratorService)} CompanyMessageHandler companyId is null.");
                    return ReturnStatus.Fail;
                }

                var getCompanyInfoDto = new GetCompanyInfoDto
                {
                    CompanyId = companyId,
                };

                return await dataService.GetCompanyDataAndUpsert(getCompanyInfoDto);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(RabbitMqYouratorService)} CompanyMessageHandler error.");
                throw;
            }
        }

        public async override Task<ReturnStatus> JobInfoMessageHandler<T>(T args)
        {

            logger.LogInformation($"{nameof(RabbitMqYouratorService)} JobInfoMessageHandler start.");

            if (args is not BasicDeliverEventArgs basicDeliverEventArgs)
            {
                logger.LogError($"{nameof(RabbitMqYouratorService)} JobInfoMessageHandler basicDeliverEventArgs is null.");
                return ReturnStatus.Fail;
            }

            try
            {
                var body = basicDeliverEventArgs.Body.ToArray();

                if (body.Length == 0)
                {
                    logger.LogError($"{nameof(RabbitMqYouratorService)} JobInfoMessageHandler body is null.");
                    return ReturnStatus.Fail;
                }

                var message = Encoding.UTF8.GetString(body);

                var simpleJobInfo = JsonSerializer.Deserialize<SimpleJobInfoDto>(message);

                if (simpleJobInfo == null || string.IsNullOrWhiteSpace(simpleJobInfo.CompanyId) || string.IsNullOrWhiteSpace(simpleJobInfo.JobId))
                {
                    logger.LogError($"{nameof(RabbitMqYouratorService)} JobInfoMessageHandler SimpleJobInfoDto is null.");
                    return ReturnStatus.Fail;
                }

                var getJobInfoDto = new GetJobInfoDto
                {
                    CompanyId = simpleJobInfo.CompanyId,
                    JobId = simpleJobInfo.JobId,
                };

                return await dataService.GetJobDataAndUpsert(getJobInfoDto);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(RabbitMqYouratorService)} JobInfoMessageHandler error.");
                throw;
            }
        }
    }
}
