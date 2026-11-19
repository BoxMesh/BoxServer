# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY dotnet-tools.json ./
RUN dotnet tool restore
COPY src/API/API.csproj src/API/
COPY src/Infra/Infra.csproj src/Infra/
RUN dotnet restore src/API/API.csproj
COPY src/ src/
RUN dotnet publish src/API/API.csproj -c Release -o /app --no-restore

RUN dotnet ef migrations bundle \
      --project src/Infra \
      --startup-project src/API \
      --configuration Release \
      --output /app/efbundle

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
EXPOSE 8080 8081
ENTRYPOINT ["/bin/sh", "-c", "./efbundle && exec dotnet API.dll"]
