# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
# Copy csproj and restore as distinct layers
COPY ["VideoStreaming.csproj", "."]
COPY . .
RUN dotnet restore "VideoStreaming.csproj"

FROM build AS publish
WORKDIR /src/
RUN dotnet publish "VideoStreaming.csproj" -c Debug -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app/run
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoStreaming.dll"]