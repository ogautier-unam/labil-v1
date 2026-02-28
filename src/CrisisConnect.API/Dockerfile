FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/CrisisConnect.API/CrisisConnect.API.csproj", "src/CrisisConnect.API/"]
COPY ["src/CrisisConnect.Application/CrisisConnect.Application.csproj", "src/CrisisConnect.Application/"]
COPY ["src/CrisisConnect.Infrastructure/CrisisConnect.Infrastructure.csproj", "src/CrisisConnect.Infrastructure/"]
COPY ["src/CrisisConnect.Domain/CrisisConnect.Domain.csproj", "src/CrisisConnect.Domain/"]
RUN dotnet restore "src/CrisisConnect.API/CrisisConnect.API.csproj"

COPY . .
RUN dotnet publish "src/CrisisConnect.API/CrisisConnect.API.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CrisisConnect.API.dll"]
