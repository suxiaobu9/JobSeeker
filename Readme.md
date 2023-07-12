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

## 環境設定

- 啟動 setup 的 docker compose

  ```ps1
  docker compose -f ./DockerService/docker-compose-setup.yaml up -d
  ```

### 設定 nacos 參數

- 開啟網站 `http://localhost:8848/nacos`，預設帳密 nacos/nacos

  1.  設定 namespace

      1. Namespace ID : `608369bd-2a9e-4a62-bdc2-b023c0d720a4`
      1. Namespace : `JobSeeker`
      1. Description : `JobSeeker`

  2.  在 namespace = JobSeeker 中設定參數

      1. Data Id = `seq`

         ```json
         {
           "SeqLogServerAddress": "http://172.20.0.2:5341/"
         }
         ```

      2. Data Id = `rabbit-mq`

         ```json
         {
           "RabbitMq": {
             "Host": "172.20.0.3",
             "Name": "guest",
             "Password": "guest"
           }
         }
         ```

      3. Data Id = `postgresql`

         ```json
         {
           "ConnectionStrings": {
             "NpgsqlConnection": "Server=172.20.0.4;Port=5432;Database=postgres;User Id=jobseeker;Password=jobseeker"
           }
         }
         ```

      4. Data Id = `redis`

         ```json
         {
           "Redis": {
             "Host": "172.20.0.9:6379",
             "Secret": "jobseekerredispwde412c4942391bb8c9a55c2fa66849a0954a761dc"
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
  curl -X GET "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=rabbit-mq&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"
  curl -X GET "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=postgresql&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"
  curl -X GET "http://127.0.0.1:8848/nacos/v1/cs/configs?dataId=redis&group=DEFAULT_GROUP&tenant=608369bd-2a9e-4a62-bdc2-b023c0d720a4&accessToken=$token"

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
