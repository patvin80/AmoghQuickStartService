#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["QuickStartService/QuickStartService.csproj", "QuickStartService/"]
RUN dotnet nuget add source https://pkgs.dev.azure.com/UpskillSDK/skylight-sdk/_packaging/release/nuget/v3/index.json -n skylightSdkRelease
RUN dotnet restore "QuickStartService/QuickStartService.csproj"
COPY . .
WORKDIR "/src/QuickStartService"
RUN dotnet build "QuickStartService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuickStartService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickStartService.dll"]