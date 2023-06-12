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
