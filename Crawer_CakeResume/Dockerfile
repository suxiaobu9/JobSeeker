#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ./Crawer_CakeResume/Crawer_CakeResume.csproj ./Crawer_CakeResume/Crawer_CakeResume.csproj
COPY ./Model/Model.csproj ./Model/Model.csproj
COPY ./Service/Service.csproj ./Service/Service.csproj

RUN dotnet restore "./Crawer_CakeResume/Crawer_CakeResume.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Crawer_CakeResume/Crawer_CakeResume.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Crawer_CakeResume/Crawer_CakeResume.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crawer_CakeResume.dll"]