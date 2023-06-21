# Job Seeker

## 專案初始化流程

1. 需要先跑起相關服務

   ```ps1
   cd DockerService
   docker compose up -d
   ```

## 指令

- 更新 EF Core entity

  ```ps1
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

- 啟動 docker compose 服務

  ```ps1
  # dev 環境
  docker compose -f docker-compose-dev.yaml up -d
  # 一般環境
  docker compose -f docker-compose.yaml up -d
  ```

- 設定 nacos 中的參數 `http://localhost:8848/nacos`

  1. 設定 namespace

     1. Namespace ID : `608369bd-2a9e-4a62-bdc2-b023c0d720a4`
     1. Namespace : `JobSeeker`
     1. Description : `JobSeeker`

  1. 在 namespace = JobSeeker 中設定參數

     1. Data Id = `seq`

        ```json
        {
          "SeqLogServerAddress": "http://172.20.0.2:5341/"
        }
        ```

     1. Data Id = `rabbit-mq`

        ```json
        {
          "RabbitMq": {
            "Host": "172.20.0.3",
            "Name": "guest",
            "Password": "guest"
          }
        }
        ```

     1. Data Id = `postgresql`

        ```json
        {
          "ConnectionStrings": {
            "NpgsqlConnection": "Server=172.20.0.4;Port=5432;Database=postgres;User Id=jobseeker;Password=jobseeker"
          }
        }
        ```
