FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VAMMS.Core/VAMMS.Core.csproj", "VAMMS.Core/"]
COPY ["VAMMS.Shared/VAMMS.Shared.csproj", "VAMMS.Shared/"]
RUN dotnet restore "VAMMS.Core/VAMMS.Core.csproj"
COPY . .
WORKDIR "/src/VAMMS.Core"
RUN dotnet build "VAMMS.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VAMMS.Core.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VAMMS.Core.dll"]