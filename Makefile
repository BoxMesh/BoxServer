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