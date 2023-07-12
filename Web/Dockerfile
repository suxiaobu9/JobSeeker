#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ./Web/Web.csproj ./Web/Web.csproj
COPY ./Model/Model.csproj ./Model/Model.csproj
COPY ./Service/Service.csproj ./Service/Service.csproj

RUN dotnet restore "./Web/Web.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Web/Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Web/Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]