#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Crawer_104/Crawer_104.csproj", "Crawer_104/"]
RUN dotnet restore "Crawer_104/Crawer_104.csproj"
COPY . .
WORKDIR "/src/Crawer_104"
RUN dotnet build "Crawer_104.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crawer_104.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crawer_104.dll"]