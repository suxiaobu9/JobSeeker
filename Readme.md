# Job Seeker

## 專案初始化流程

1. 建立一個 docker network

   ```ps1
   docker network create --subnet=172.20.0.0/16 job_seeker_network
   ```

2. 跑一個 seq log server

   ```ps1
   docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y --network job_seeker_network --ip 172.20.0.2 -p 5341:80 -v D:\docker\seq:/data datalust/seq:latest
   ```

3. 跑一個 rabbitmq

   ```ps1
   docker run -it --name rabbitmq -d --restart unless-stopped --network job_seeker_network --ip 172.20.0.3 -p 5672:5672 -p 15672:15672 -v D:\docker\rabbitmq:/var/lib/rabbitmq rabbitmq:3.12-management
   ```
