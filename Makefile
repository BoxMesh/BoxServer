-include .env
export

server:
	dotnet run --project src/EdgePanel.API --verbosity quiet
migration:
	@if [ -z "$(Name)" ]; then \
		echo "Usage: make migration Name=InitDB"; \
		exit 1; \
	fi
	dotnet ef migrations add $(Name) \
		--project src/EdgePanel.Infrastructure \
		--startup-project src/EdgePanel.API \
		--output-dir Database/Migrations
migrate:
	dotnet ef database update --project src/EdgePanel.Infrastructure --startup-project src/EdgePanel.API
admin:
	@if [ -z "$(Email)" ] || [ -z "$(Password)" ]; then \
		echo "Usage: make admin Email=a@b.c Password=secret"; \
		exit 1; \
	fi
	dotnet run --project src/EdgePanel.CLI -- create-admin --email $(Email) --password $(Password)
