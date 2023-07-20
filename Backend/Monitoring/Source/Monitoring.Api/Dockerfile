FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["Source/Monitoring.Api/Monitoring.Api.csproj", "Source/Monitoring.Api/"]
COPY ["Source/Monitoring.Domain/Monitoring.Domain.csproj", "Source/Monitoring.Domain/"]
COPY ["Source/Monitoring.Contracts/Monitoring.Contracts.csproj", "Source/Monitoring.Contracts/"]
COPY ["Source/Monitoring.Shared/Monitoring.Shared.csproj", "Source/Monitoring.Shared/"]
COPY ["Source/Monitoring.Dal/Monitoring.Dal.csproj", "Source/Monitoring.Dal/"]
RUN dotnet restore "Source/Monitoring.Api/Monitoring.Api.csproj"
COPY . .
WORKDIR "/src/Source/Monitoring.Api"
RUN dotnet build "Monitoring.Api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Monitoring.Api.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Monitoring.Api.dll"]
