FROM mcr.microsoft.com/mssql-tools

WORKDIR /app

COPY sql-migration-scripts/migration.sql .

RUN echo "📦 Ensuring database [$DB_NAME] exists..."

CMD /opt/mssql-tools/bin/sqlcmd -S $DB_HOST -U $DB_USER -P $DB_PASS -d master -Q "IF DB_ID('$DB_NAME') IS NULL CREATE DATABASE [$DB_NAME];"

CMD /opt/mssql-tools/bin/sqlcmd \
  -S $DB_HOST -U $DB_USER -P $DB_PASS -d $DB_NAME \
  -i migration.sql


COPY sql-migration-scripts/migration.sql .
COPY cicd/scripts/migration-entrypoint.sh .

RUN chmod +x .migration-entrypoint.sh

ENTRYPOINT ["/app/migration-entrypoint.sh"]