-include .env
export

server:
	dotnet run --project src/BoxServer.API
migration:
	@if [ -z "$(Name)" ]; then \
		echo "Usage: make migration Name=InitDB"; \
		exit 1; \
	fi
	dotnet ef migrations add $(Name) \
		--project src/BoxServer.Infra \
		--startup-project src/BoxServer.API \
		--output-dir Database/Migrations
migrate:
	dotnet ef database update --project src/BoxServer.Infra --startup-project src/BoxServer.API
