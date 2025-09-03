FROM mcr.microsoft.com/mssql-tools

WORKDIR /app

RUN ls -lrta 

COPY runner/download/migrations/migration.sql .
COPY cicd/scripts/migration-entrypoint.sh .

RUN chmod +x /app/migration-entrypoint.sh

ENTRYPOINT ["/app/migration-entrypoint.sh"]