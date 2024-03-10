FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY WeatherWatch.Api.sln .
COPY WeatherWatch.Api/*.csproj ./WeatherWatch.Api/
COPY WeatherWatch.Domain/*.csproj ./WeatherWatch.Domain/
COPY WeatherBit.Domain/*.csproj ./WeatherBit.Domain/
RUN dotnet restore

COPY . .
RUN dotnet publish WeatherWatch.Api.sln -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "WeatherWatch.Api.dll"]
