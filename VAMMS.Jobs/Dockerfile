FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VAMMS.Jobs/VAMMS.Jobs.csproj", "VAMMS.Jobs/"]
COPY ["VAMMS.Shared/VAMMS.Shared.csproj", "VAMMS.Shared/"]
RUN dotnet restore "VAMMS.Jobs/VAMMS.Jobs.csproj"
COPY . .
WORKDIR "/src/VAMMS.Jobs"
RUN dotnet build "VAMMS.Jobs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VAMMS.Jobs.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VAMMS.Jobs.dll"]