# Zin Server

Zin Server is the backend of Zin. It does two jobs:

- **gRPC (port 8081):** receives a report from each Zin Exporter (the agent) every few seconds and stores it.
- **HTTP API (port 8080):** serves the Zin Portal (the web console) with sign-in, devices, groups, users and enrollment tokens.

```text
Exporter (each machine) ──gRPC──► Zin Server ◄──HTTPS── Portal (browser)
                                      │
                                  PostgreSQL
```

## Projects

| Project | What it is |
|---|---|
| `src/Zin.API` | ASP.NET Core host: REST controllers, the gRPC `DeviceGRPC.Ping` service, JWT auth, OpenAPI |
| `src/Zin.Infra` | EF Core (PostgreSQL), entities, migrations, repositories and services |
| `src/Zin.CLI` | Command-line tool; `create-admin` makes the first System account |

Requires .NET 10 and PostgreSQL 18.

## Run it

```bash
cp .env.example .env              # fill in the values below
docker compose -f docker/compose.yml up -d   # PostgreSQL (and Redis); needs docker/.env, see docker/.env.docker.example
make migrate                      # apply the database migrations
dotnet run --project src/Zin.CLI -- create-admin --email you@example.com --password '...'
make server                       # API on :8080, gRPC on :8081
```

### Configuration (`.env`)

| Key | Meaning |
|---|---|
| `ConnectionStrings__PostgreSQL` | PostgreSQL connection string |
| `Jwt__Issuer`, `Jwt__Audience`, `Jwt__SecretKey` | JWT settings. The token travels in an HttpOnly cookie. Use a long random secret |
| `Kestrel__Endpoints__Http__Url` | HTTP listener, default `http://+:8080` |
| `Kestrel__Endpoints__Grpc__Url` | gRPC listener, default `http://+:8081` (HTTP/2) |
| `ASPNETCORE_ENVIRONMENT` | Set `Development` to turn on Swagger UI at `/swagger` |

## API

Sign-in and sign-out are open; everything else needs the session cookie. Roles are **System** (everything) and **User**
(only the devices granted to them).

| Method | Endpoint | Who |
|---|---|---|
| POST | `/log-in`, `/log-out`, `/sign-in` | anyone |
| PATCH | `/account` | signed in |
| GET | `/devices`, `/devices/{id}`, `/devices/{id}/metrics?range=1h\|24h\|7d`, `/devices/{id}/processes` | signed in; a User sees only granted devices |
| PATCH | `/devices/{id}` (alias) | signed in |
| PUT | `/devices/{id}/groups` | System |
| GET | `/groups`, `/groups/{id}` | signed in |
| POST, PATCH, DELETE | `/groups`, `/groups/{id}` | System |
| GET, POST, PATCH, DELETE | `/users`, `/users/{id}` | System |
| GET, POST, DELETE | `/enrollment-tokens`, `/enrollment-tokens/{id}` | System |

Responses are wrapped as `{ "data": ... }` on success and `{ "error": ... }` on failure.

## gRPC

`device.DeviceGRPC/Ping` takes one report per call (identity, hardware, usage, every process). The server creates the
device on the first call and updates it after that. The contract lives in `src/Zin.API/Protos/Device.proto` and must stay
identical to `proto/device.proto` in the exporter repo; change both together.

The gRPC port has no TLS and does not check who calls it yet.

## Database

`make migration Name=Something` adds a migration; `make migrate` applies it. Metrics older than the retention window are
pruned by the `MetricRetention` background service.

## Docker

```bash
docker build -t zin-server .
```

The image applies migrations (`efbundle`) on start, then runs `Zin.API`. It exposes 8080 and 8081.
