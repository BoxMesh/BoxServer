FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/EdgePanel.API/EdgePanel.API.csproj src/EdgePanel.API/
RUN dotnet restore src/EdgePanel.API/EdgePanel.API.csproj
COPY src/ src/
RUN dotnet publish src/EdgePanel.API/EdgePanel.API.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["dotnet", "EdgePanel.API.dll"]
