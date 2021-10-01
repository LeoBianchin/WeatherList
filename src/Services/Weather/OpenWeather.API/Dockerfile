# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
# ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY WeatherList.sln ./

COPY BuildingBlocks/EventBus/EventBus/*.csproj ./BuildingBlocks/EventBus/EventBus/
COPY Services/Weather/OpenWeather.API/*.csproj ./Services/Weather/OpenWeather.API/
COPY Services/Playlist/Playlist.API/*.csproj ./Services/Playlist/Playlist.API/
COPY Web/WeatherListSPA/*.csproj ./Web/WeatherListSPA/

RUN dotnet restore
# copy csproj and restore as distinct layers
COPY . .

WORKDIR /src/BuildingBlocks/EventBus/EventBus
RUN dotnet build -c Release -o /app

WORKDIR /src/Services/Weather/OpenWeather.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

# final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/ .
ENTRYPOINT ["dotnet", "OpenWeather.API.dll"]