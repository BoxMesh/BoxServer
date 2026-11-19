# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY dotnet-tools.json ./
RUN dotnet tool restore
COPY src/Zin.API/Zin.API.csproj src/Zin.API/
COPY src/Zin.Infra/Zin.Infra.csproj src/Zin.Infra/
RUN dotnet restore src/Zin.API/Zin.API.csproj
COPY src/ src/
RUN dotnet publish src/Zin.API/Zin.API.csproj -c Release -o /app --no-restore

RUN dotnet ef migrations bundle \
      --project src/Zin.Infra \
      --startup-project src/Zin.API \
      --configuration Release \
      --output /app/efbundle

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
EXPOSE 8080 8081
ENTRYPOINT ["/bin/sh", "-c", "./efbundle && exec dotnet Zin.API.dll"]
