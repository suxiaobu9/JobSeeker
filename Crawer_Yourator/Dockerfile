#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ./Crawer_Yourator/Crawer_Yourator.csproj ./Crawer_Yourator/Crawer_Yourator.csproj
COPY ./Model/Model.csproj ./Model/Model.csproj
COPY ./Service/Service.csproj ./Service/Service.csproj

RUN dotnet restore "./Crawer_Yourator/Crawer_Yourator.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Crawer_Yourator/Crawer_Yourator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Crawer_Yourator/Crawer_Yourator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crawer_Yourator.dll"]