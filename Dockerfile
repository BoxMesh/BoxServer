# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY dotnet-tools.json ./
RUN dotnet tool restore
COPY src/BoxServer.API/BoxServer.API.csproj src/BoxServer.API/
COPY src/BoxServer.Infra/BoxServer.Infra.csproj src/BoxServer.Infra/
RUN dotnet restore src/BoxServer.API/BoxServer.API.csproj
COPY src/ src/
RUN dotnet publish src/BoxServer.API/BoxServer.API.csproj -c Release -o /app --no-restore

RUN dotnet ef migrations bundle \
      --project src/BoxServer.Infra \
      --startup-project src/BoxServer.API \
      --configuration Release \
      --output /app/efbundle

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
EXPOSE 8080 8081
ENTRYPOINT ["/bin/sh", "-c", "./efbundle && exec dotnet BoxServer.API.dll"]
