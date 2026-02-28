FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/CrisisConnect.Web/CrisisConnect.Web.csproj", "src/CrisisConnect.Web/"]
RUN dotnet restore "src/CrisisConnect.Web/CrisisConnect.Web.csproj"
COPY . .
RUN dotnet publish "src/CrisisConnect.Web/CrisisConnect.Web.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CrisisConnect.Web.dll"]
