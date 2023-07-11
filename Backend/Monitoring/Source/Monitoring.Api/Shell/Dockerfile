FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore ./Source/Monitoring.Api/Monitoring.Api.csproj
RUN dotnet publish -c Debug -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_ENVIRONMENT="Development"
ENTRYPOINT ["dotnet", "Monitoring.Api.dll"]
