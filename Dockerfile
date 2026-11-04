# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY dotnet-tools.json ./
RUN dotnet tool restore
COPY src/EdgePanel.API/EdgePanel.API.csproj src/EdgePanel.API/
COPY src/EdgePanel.Infrastructure/EdgePanel.Infrastructure.csproj src/EdgePanel.Infrastructure/
RUN dotnet restore src/EdgePanel.API/EdgePanel.API.csproj
COPY src/ src/
RUN dotnet publish src/EdgePanel.API/EdgePanel.API.csproj -c Release -o /app --no-restore

RUN dotnet ef migrations bundle \
      --project src/EdgePanel.Infrastructure \
      --startup-project src/EdgePanel.API \
      --configuration Release \
      --output /app/efbundle

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["/bin/sh", "-c", "./efbundle && exec dotnet EdgePanel.API.dll"]
