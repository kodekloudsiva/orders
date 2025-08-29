FROM mcr.microsoft.com/mssql-tools

WORKDIR /app

COPY sql-migration-scripts/migration.sql .
COPY cicd/scripts/migration-entrypoint.sh .

RUN chmod +x .migration-entrypoint.sh

ENTRYPOINT ["/app/migration-entrypoint.sh"]