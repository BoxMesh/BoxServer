-include .env
export

server:
	dotnet run --project src/Zin.API
migration:
	@if [ -z "$(Name)" ]; then \
		echo "Usage: make migration Name=InitDB"; \
		exit 1; \
	fi
	dotnet ef migrations add $(Name) \
		--project src/Zin.Infra \
		--startup-project src/Zin.API \
		--output-dir Database/Migrations
migrate:
	dotnet ef database update --project src/Zin.Infra --startup-project src/Zin.API
