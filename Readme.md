# Job Seeker

## 專案啟動

```ps1
 cd DockerService
# setup 環境
docker compose -f docker-compose-setup.yaml up -d
# dev 環境
docker compose -f docker-compose-dev.yaml up -d
# 一般環境
docker compose -f docker-compose.yaml up -d
```

## 指令

- 更新 EF Core entity

  ```ps1
  cd Model
  $connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=jobseeker;Password=jobseeker"
  dotnet ef dbcontext scaffold $connectionString Npgsql.EntityFrameworkCore.PostgreSQL -o ./JobSeekerDb --force
  ```

- Migration

  ```ps1
  cd Model
  # 新增 建立 migration 文件
  dotnet ef migrations add InitialCreate --context Model.JobSeekerDb.postgresContext
  # 新增 更新 migration 文件
  dotnet ef migrations add <name> --context Model.JobSeekerDb.postgresContext
  ```

- build docker image

  ```ps1
    docker build -f ./Crawer_104/Dockerfile -t arisuokay/job-seeker-crawer-104:v2.30 -t arisuokay/job-seeker-crawer-104:latest .
    docker push arisuokay/job-seeker-crawer-104:v2.30
    docker push arisuokay/job-seeker-crawer-104:latest

    docker build -f ./Crawer_CakeResume/Dockerfile -t arisuokay/job-seeker-crawer-cakeresume:v2.27 -t arisuokay/job-seeker-crawer-cakeresume:latest .
    docker push arisuokay/job-seeker-crawer-cakeresume:v2.27
    docker push arisuokay/job-seeker-crawer-cakeresume:latest

    docker build -f ./Crawer_Yourator/Dockerfile -t arisuokay/job-seeker-crawer-yourator:v2.5 -t arisuokay/job-seeker-crawer-yourator:latest .
    docker push arisuokay/job-seeker-crawer-yourator:v2.5
    docker push arisuokay/job-seeker-crawer-yourator:latest

    docker build -f ./Crawer_1111/Dockerfile -t arisuokay/job-seeker-crawer-1111:v2.9 -t arisuokay/job-seeker-crawer-1111:latest .
    docker push arisuokay/job-seeker-crawer-1111:v2.9
    docker push arisuokay/job-seeker-crawer-1111:latest

    docker build -f ./Web/Dockerfile -t arisuokay/job-seeker-web:v1.14 -t arisuokay/job-seeker-web:latest .
    docker push arisuokay/job-seeker-web:v1.14
    docker push arisuokay/job-seeker-web:latest
  ```

## 環境設定

- 啟動 setup 的 docker compose

  ```ps1
  docker compose -f ./DockerService/docker-compose-setup.yaml up -d
  ```

### 設定 nacos 參數

- ConnectionString 如果有 `+` 號要換成 `%2B`

- 開啟網站 `http://localhost:8848/nacos`，預設帳密 nacos/nacos

  1. 設定 namespace

     1. Namespace ID : `608369bd-2a9e-4a62-bdc2-b023c0d720a4`
     2. Namespace : `JobSeeker`
     3. Description : `JobSeeker`

  2. 在 namespace = JobSeeker 中設定參數

     1. Data Id = `seq`

        ```json
        {
          "SeqLogServerAddress": "http://172.20.0.2:5341/"
        }
        ```

     2. Data Id = `postgresql`

        ```json
        {
          "ConnectionStrings": {
            "NpgsqlConnection": "Server=172.20.0.4;Port=5432;Database=postgres;User Id=jobseeker;Password=jobseeker"
          }
        }
        ```

     3. Data Id = `redis`

        ```json
        {
          "Redis": {
            "Host": "172.20.0.9:6379",
            "Secret": "jobseekerredispwde412c4942391bb8c9a55c2fa66849a0954a761dc"
          }
        }
        ```

     4. Data Id = `rabbit-mq`

        ```json
        {
          "RabbitMq": {
            "Host": "172.20.0.3",
            "Name": "guest",
            "Password": "guest"
          }
        }
        ```

- 利用指令新增

  ```bash
  token=$(curl -X POST 'http://127.0.0.1:8848/nacos/v1/auth/login' -d 'username=nacos&password=nacos' | grep -o '"accessToken":"[^"]*' | awk -F ':"' '{print $2}')

  curl -X POST 'http://127.0.0.1:8848/nacos/v1/console/namespaces' -d "customNamespaceId=608369bd-2a9e-4a62-bdc2-b023c0d720a4&namespaceName=JobSeeker&namespaceDesc=JobSeeker&accessToken=$token"

  curl -X POST 'http://127.0.0.1:8848/nacos/v1/cs/configs' -d "dataId=seq&group=DEFAULT_GROUP&type=json&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&content={\"SeqLogServerAddress\":\"http://172.20.0.2:5341/\"}&accessToken=$token"
  curl -X POST 'http://127.0.0.1:8848/nacos/v1/cs/configs' -d "dataId=rabbit-mq&group=DEFAULT_GROUP&type=json&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&content={\"RabbitMq\":{\"Host\":\"172.20.0.3\",\"Name\":\"guest\",\"Password\":\"guest\"}}&accessToken=$token"
  curl -X POST 'http://127.0.0.1:8848/nacos/v1/cs/configs' -d "dataId=postgresql&group=DEFAULT_GROUP&type=json&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&content={\"ConnectionStrings\":{\"NpgsqlConnection\":\"Server=172.20.0.4;Port=5432;Database=postgres;User Id=jobseeker;Password=jobseeker\"}}&accessToken=$token"
  curl -X POST 'http://127.0.0.1:8848/nacos/v1/cs/configs' -d "dataId=redis&group=DEFAULT_GROUP&type=json&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&content={\"Redis\":{\"Host\":\"172.20.0.9:6379\",\"Secret\":\"jobseekerredispwde412c4942391bb8c9a55c2fa66849a0954a761dc\"}}&accessToken=$token"

  curl -X GET "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=seq&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"
  curl -X GET "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=postgresql&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"
  curl -X GET "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=redis&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"
  curl -X GET "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=rabbit-mq&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"

  ```

  ```ps1
  $nacosResponse = Invoke-RestMethod -Method POST -Uri 'http://127.0.0.1:8848/nacos/v1/auth/login' -Body @{
    username = 'nacos'
    password = 'nacos'
  }

  $token = $nacosResponse.accessToken

  Invoke-RestMethod -Method POST -Uri 'http://127.0.0.1:8848/nacos/v1/console/namespaces' -Body @{
    customNamespaceId = '608369bd-2a9e-4a62-bdc2-b023c0d720a4'
    namespaceName = 'JobSeeker'
    namespaceDesc = 'JobSeeker'
    type = 'json'
    accessToken = $token
  }

  Invoke-RestMethod -Method POST -Uri 'http://127.0.0.1:8848/nacos/v1/cs/configs' -Body @{
      dataId = 'seq'
      group = 'DEFAULT_GROUP'
      tenant = '608369bd-2a9e-4a62-bdc2-b023c0d720a4'
      content = '{"SeqLogServerAddress":"http://172.20.0.2:5341/"}'
      type = 'json'
      accessToken = $token
  }

  Invoke-RestMethod -Method POST -Uri 'http://127.0.0.1:8848/nacos/v1/cs/configs' -Body @{
      dataId = 'rabbit-mq'
      group = 'DEFAULT_GROUP'
      tenant = '608369bd-2a9e-4a62-bdc2-b023c0d720a4'
      content = '{"RabbitMq":{"Host":"172.20.0.3","Name":"guest","Password":"guest"}}'
      type = 'json'
      accessToken = $token
  }

  Invoke-RestMethod -Method POST -Uri 'http://127.0.0.1:8848/nacos/v1/cs/configs' -Body @{
      dataId = 'postgresql'
      group = 'DEFAULT_GROUP'
      tenant = '608369bd-2a9e-4a62-bdc2-b023c0d720a4'
      content = '{"ConnectionStrings":{"NpgsqlConnection":"Server=172.20.0.4;Port=5432;Database=postgres;User Id=jobseeker;Password=jobseeker"}}'
      type = 'json'
      accessToken = $token
  }

  Invoke-RestMethod -Method POST -Uri 'http://127.0.0.1:8848/nacos/v1/cs/configs' -Body @{
      dataId = 'redis'
      group = 'DEFAULT_GROUP'
      tenant = '608369bd-2a9e-4a62-bdc2-b023c0d720a4'
      content = '{"Redis":{"Host":"172.20.0.9:6379","Secret":"jobseekerredispwde412c4942391bb8c9a55c2fa66849a0954a761dc"}}'
      type = 'json'
      accessToken = $token
  }

  Invoke-RestMethod -Method GET -Uri "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=seq&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"
  Invoke-RestMethod -Method GET -Uri "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=rabbit-mq&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"
  Invoke-RestMethod -Method GET -Uri "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=postgresql&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"
  Invoke-RestMethod -Method GET -Uri "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=redis&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"

  ```

### 啟動服務

```ps1
docker compose -f ./DockerService/docker-compose.yaml up -d
```

## 封存區

### 利用 azure cli 建立 azure service bus 服務並取得連線字串，再更新到 nacos

```ps1
# Azure Active Directory 中應用程式註冊的 憑證及秘密
$Password = 'p*********************~'

# Azure Active Directory 中應用程式註冊的 應用程式 (用戶端) 識別碼
$ApplicationId = '{GUID}'

# Azure Active Directory 概觀畫面的 租用戶識別碼
$TenantId = '{GUID}'
az login --service-principal -u $ApplicationId -p $Password --tenant $TenantId

# 需要將 應用程式 加入到 資源群組 存取控制 (IAM) 的角色指派，並給參與者 (在 具有特殊權限的系統管理員角色 頁簽下)
$QueueName = 'JobSeekerCrawer-dev'
$SourceGroupName = 'Demo'
$AuthorizationRule = 'ReadWrite'

# 建立 Service Bus
az servicebus namespace create --resource-group $SourceGroupName --name $QueueName --location eastasia --sku Basic
az servicebus namespace authorization-rule create --resource-group $SourceGroupName --namespace-name $QueueName --name $AuthorizationRule --rights Send Listen

# 建立 Queue
az servicebus queue create --name queue_company_id_for_104 --namespace-name $QueueName --resource-group $SourceGroupName
az servicebus queue create --name queue_job_id_for_104 --namespace-name $QueueName --resource-group $SourceGroupName

$ServiceBusConnectionString = az servicebus namespace authorization-rule keys list --resource-group $SourceGroupName --namespace-name $QueueName --name $AuthorizationRule --query primaryConnectionString --output tsv

# 移除 Service Bus
# az servicebus namespace delete --resource-group $SourceGroupName --name $QueueName
```

### 利用指令將 Azure Service Bus 的連線字串新增到 nacos

- 需要先變更 `AzureServiceBus` 的 ConnectionString

- Data Id = `azure-service-bus`

  ```json
  {
    "AzureServiceBus": {
      "ConnectionString": "<your connection string>"
    }
  }
  ```

```bash
  curl -X POST 'http://127.0.0.1:8848/nacos/v1/cs/configs' -d "dataId=azure-service-bus&group=DEFAULT_GROUP&type=json&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&content={\"AzureServiceBus\":{\"ConnectionString\":\"<your connection string>\"}}&accessToken=$token"


  curl -X GET "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=azure-service-bus&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"

```

```ps1
  Invoke-RestMethod -Method POST -Uri 'http://127.0.0.1:8848/nacos/v1/cs/configs' -Body @{
      dataId = 'azure-service-bus'
      group = 'DEFAULT_GROUP'
      tenant = '608369bd-2a9e-4a62-bdc2-b023c0d720a4'
      content = '{"AzureServiceBus":{"ConnectionString":"<your connection string>"}}'
      type = 'json'
      accessToken = $token
  }

  Invoke-RestMethod -Method GET -Uri "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=azure-service-bus&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"
```
